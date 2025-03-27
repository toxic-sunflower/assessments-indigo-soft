using AccessTracker.Application.EventTracking;
using AccessTracker.Application.EventTracking.Services;
using AccessTracker.Domain.AccessLog;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Time.Testing;
using Moq;

namespace AccessTracker.Test.Application.EventTracking;


public class AccessEventTrackerTest : TestBase<AccessEventTracker>
{
    private const int RetryAttempts = 3;
    private const long UserId = 117;
    private const string IpAddress = "192.168.1.1";

    
    [Fact(DisplayName = "Должен создавать корректную запись и сохранять её")]
    public async Task Should_Create_Correct_Entry_And_Save_It()
    {
        // Act
        var result = await Target.TrackAccessEventAsync(
            new AccessEvent(UserId, IpAddress),
            CancellationToken.None);
        
        
        // Assert
        result.IsError.Should().BeFalse();
        
        _accessLogRepository.Verify(
            x => x.AddEntryAsync(
                It.Is<AccessLogEntry>(
                    e => e.UserId == UserId &&
                         e.IpAddress == IpAddress &&
                         e.Timestamp == _fakeTimeProvider.GetUtcNow()),
                It.IsAny<CancellationToken>()),
            Times.Exactly(1));
    }
    
    [Fact(DisplayName = "Должен повторять попытку сохранения количество указанное в настройках")]
    public async Task Should_Retry_Expected_Times()
    {
        // Arrange
        _accessLogRepository
            .Setup(
                x => x.AddEntryAsync(
                    It.IsAny<AccessLogEntry>(),
                    It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception());
        
        
        // Act
        await Target.TrackAccessEventAsync(
            new AccessEvent(UserId, IpAddress),
            CancellationToken.None);
        
        
        // Assert
        _accessLogRepository
            .Verify(
                x => x.AddEntryAsync(
                    It.Is<AccessLogEntry>(
                        e => e.UserId == UserId &&
                             e.IpAddress == IpAddress &&
                             e.Timestamp == _fakeTimeProvider.GetUtcNow()),
                    It.IsAny<CancellationToken>()),
                Times.Exactly(RetryAttempts + 1));
    }
    
    [Fact(DisplayName = "Должен обрабатывать полный провал попытки сохранения")]
    public async Task Should_Handle_Failure()
    {
        var exception = new Exception();
        
        // Arrange
        _accessLogRepository
            .Setup(
                x => x.AddEntryAsync(
                    It.IsAny<AccessLogEntry>(),
                    It.IsAny<CancellationToken>()))
            .ThrowsAsync(exception);
        
        
        // Act
        var result = await Target.TrackAccessEventAsync(
            new AccessEvent(UserId, IpAddress),
            CancellationToken.None);
        
        
        // Assert
        result.IsError.Should().BeTrue();
        
        _accessEventTrackFailedHandler
            .Verify(
                x => x.HandleTrackFailureAsync(
                    exception,
                    UserId,
                    IpAddress,
                    _fakeTimeProvider.GetUtcNow(),
                    It.IsAny<CancellationToken>()),
                Times.Exactly(1));
    }


    protected override void ConfigureServices(IServiceCollection services)
    {
        services.Configure<AccessEventTrackingOptions>(
            x =>
            {
                x.AttemptsCount = RetryAttempts;
                x.RetryDelay = TimeSpan.Zero;
            });

        services.AddSingleton(_accessLogRepository.Object);
        services.AddSingleton(_accessEventTrackFailedHandler.Object);
        services.AddSingleton<TimeProvider>(_fakeTimeProvider);
        
        _fakeTimeProvider.SetUtcNow(DateTimeOffset.UtcNow);
    }
    
    
    private readonly FakeTimeProvider _fakeTimeProvider = new ();
    private readonly Mock<IAccessLogRepository> _accessLogRepository = new ();
    private readonly Mock<IAccessEventTrackFailedHandler> _accessEventTrackFailedHandler = new ();
}