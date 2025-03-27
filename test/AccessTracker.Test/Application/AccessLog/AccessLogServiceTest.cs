using AccessTracker.Application.AccessLog;
using AccessTracker.Domain.AccessLog;
using AccessTracker.Domain.AccessLog.Aggregations.LastAccess;
using AccessTracker.Domain.AccessLog.Aggregations.UserIpLink;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace AccessTracker.Test.Application.AccessLog;


public class AccessLogServiceTest : TestBase<AccessLogService>
{
    [Fact(DisplayName = "SearchUsersAsync вызыввает хранилище с корректными аргументами и возвращает результат")]
    public async Task SearchUsersAsync_Should_Return_AccessLog_From_Storage_With_Correct_Parameters()
    {
        // Arrange
        var query = new UserSearchQuery("127.0.0.1", 10, 10);
        var (totalCountResult, itemsResult) = (153L, new List<long>());

        _ipLinkAggregationRepository
            .Setup(x => x.SearchUsersByIpAddressAsync(
                query.IpAddress,
                query.Skip,
                query.Take,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((totalCountResult, itemsResult));
        
        
        //Act
        var result = await Target.SearchUsersAsync(query, CancellationToken.None);
        
        
        // Assert
        result.IsError.Should().BeFalse();

        result.Value.TotalCount.Should().Be(totalCountResult);
        result.Value.UserIds.Should().BeEquivalentTo(itemsResult);
        
        _ipLinkAggregationRepository
            .Verify(
                x => x.SearchUsersByIpAddressAsync(
                    query.IpAddress,
                    query.Skip,
                    query.Take,
                    It.IsAny<CancellationToken>()),
                Times.Once());
    }
    
    [Fact(DisplayName = "GetLastAccessAsync вызыввает хранилище с корректными аргументами и возвращает результат")]
    public async Task GetLastAccessAsync_Should_Return_AccessLog_From_Storage_With_Correct_Parameters()
    {
        // Arrange
        const long userId = 100;
        var access = new UserAccess(100, DateTimeOffset.UtcNow, "127.0.0.1");

        _lastAccessAggregationRepository
            .Setup(x => x.GetByUserIdAsync(
                userId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(access);
        
        
        //Act
        var result = await Target.GetLastAccessAsync(userId, CancellationToken.None);
        
        
        // Assert
        result.IsError.Should().BeFalse();

        result.Value.UserId.Should().Be(access.UserId);
        result.Value.IpAddress.Should().Be(access.IpAddress);
        result.Value.AccessUtcTime.Should().Be(access.AccessUtcTime);
        
        _lastAccessAggregationRepository
            .Verify(
                x => x.GetByUserIdAsync(
                    userId,
                    It.IsAny<CancellationToken>()),
                Times.Once);
    }
    
    [Fact(DisplayName = "GetUserIpListAsync вызыввает хранилище с корректными аргументами и возвращает результат")]
    public async Task GetUserIpListAsync_Should_Return_AccessLog_From_Storage_With_Correct_Parameters()
    {
        // Arrange
        const long userId = 100;
        var query = new UserIpListQuery(userId, 10, 10);
        var (totalCountResult, itemsResult) = (153L, new List<string>());

        _ipLinkAggregationRepository
            .Setup(x => x.GetUserAccessedFromIpListAsync(
                query.UserId,
                query.Skip,
                query.Take,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((totalCountResult, itemsResult));
        
        
        //Act
        var result = await Target.GetUserIpListAsync(query, CancellationToken.None);
        
        
        // Assert
        result.IsError.Should().BeFalse();

        result.Value.TotalCount.Should().Be(totalCountResult);
        result.Value.Addresses.Should().BeEquivalentTo(itemsResult);
        
        _ipLinkAggregationRepository
            .Verify(
                x => x.GetUserAccessedFromIpListAsync(
                    query.UserId,
                    query.Skip,
                    query.Take,
                    It.IsAny<CancellationToken>()),
                Times.Once());
    }
    
    
    protected override void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton(_lastAccessAggregationRepository.Object);
        services.AddSingleton(_ipLinkAggregationRepository.Object);
    }
    
    
    private readonly Mock<IAccessLogLastAccessAccessLogAggregationRepository> _lastAccessAggregationRepository = new();
    private readonly Mock<IAccessLogUserIpLinkAccessLogAggregationRepository> _ipLinkAggregationRepository = new();
}