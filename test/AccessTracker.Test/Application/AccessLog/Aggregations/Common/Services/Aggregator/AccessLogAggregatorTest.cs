using AccessTracker.Application.AccessLog.Aggregations;
using AccessTracker.Application.AccessLog.Aggregations.Common.Services.Aggregator;
using AccessTracker.Domain.AccessLog;
using AccessTracker.Domain.AccessLog.Aggregations.LastAccess;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace AccessTracker.Test.Application.AccessLog.Aggregations.Common.Services.Aggregator;


public class AccessLogAggregatorTest : TestBase<AccessLogAggregator<AccessLogLastAccessAggregation>>
{
    [Fact(DisplayName = "Загружает корректные данные и передаёт их сервису обновления")]
    public async Task Should_Load_Correct_AccessLog_Entries_And_Pass_It_ToUpdater()
    {
        // Arrange
        var entries = Array.Empty<AccessLogEntry>();
        
        _entriesLoader
            .Setup(x => x.LoadEntriesAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(entries);

        
        // Act
        await Target.AggregateAsync(CancellationToken.None);
        
        
        // Assert
        _entriesLoader
            .Verify(
                x => x.LoadEntriesAsync(
                    It.IsAny<int>(),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        
        _aggregationUpdater
            .Verify(
                x => x.UpdateAggregationAsync(
                    entries,
                    It.IsAny<CancellationToken>()),
                Times.Once);
    }
    
    
    protected override void ConfigureServices(IServiceCollection services)
    {
        services.Configure<AccessLogAggregatorOptions<AccessLogLastAccessAggregation>>(_ => { });
        services.AddSingleton(_entriesLoader.Object);
        services.AddSingleton(_aggregationUpdater.Object);
    }


    private readonly Mock<IAccessLogAggregationUpdater<AccessLogLastAccessAggregation>> _aggregationUpdater = new ();
    private readonly Mock<IAccessLogAggregatorEntriesLoader<AccessLogLastAccessAggregation>> _entriesLoader = new ();
    private readonly Mock<IAccessLogRepository> _accessLogRepository = new ();
}