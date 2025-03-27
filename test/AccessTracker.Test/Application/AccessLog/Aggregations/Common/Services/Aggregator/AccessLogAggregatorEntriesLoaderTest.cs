using System.Runtime.ExceptionServices;
using AccessTracker.Application.AccessLog.Aggregations.Common.Services;
using AccessTracker.Application.AccessLog.Aggregations.Common.Services.Aggregator;
using AccessTracker.Domain.AccessLog;
using AccessTracker.Domain.AccessLog.Aggregations.Common.Checkpoints;
using AccessTracker.Domain.AccessLog.Aggregations.LastAccess;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace AccessTracker.Test.Application.AccessLog.Aggregations.Common.Services.Aggregator;


public class AccessLogAggregatorEntriesLoaderTest :
    TestBase<AccessLogAggregatorEntriesLoader<AccessLogLastAccessAggregation>>
{
    [Fact(DisplayName = "Должен получить ID последнего аггрегированного события и загрузить данные после него")]
    public async Task Should_Get_Last_Aggregated_Event_And_Get_Entries_After_It()
    {
        // Arrange
        const int count = 100;
        const int lastAggregatedEventId = 10;
        var aggregationType = _mapper.GetAggregationType<AccessLogLastAccessAggregation>();

        _checkpointRepository
            .Setup(x => x.GetLastAggregatedEventIdAsync(aggregationType, It.IsAny<CancellationToken>()))
            .ReturnsAsync(lastAggregatedEventId);
        
        
        // Act
        var entries = await Target.LoadEntriesAsync(100, CancellationToken.None);
        
        
        // Assert
        _checkpointRepository.Verify(
            x => x.GetLastAggregatedEventIdAsync(aggregationType, It.IsAny<CancellationToken>()),
            Times.Once);
        
        _accessLogRepository.Verify(
            x => x.GetWithIdGreaterThanAsync(lastAggregatedEventId, count, It.IsAny<CancellationToken>()),
            Times.Once);
    }

    
    protected override void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton(_mapper);
        services.AddSingleton(_checkpointRepository.Object);
        services.AddSingleton(_accessLogRepository.Object);
    }


    private readonly IAccessLogAggregationTypeMapper _mapper = new AccessLogAggregationTypeMapper();
    private readonly Mock<IAccessLogAggregationCheckpointRepository> _checkpointRepository = new ();
    private readonly Mock<IAccessLogRepository> _accessLogRepository = new ();
}