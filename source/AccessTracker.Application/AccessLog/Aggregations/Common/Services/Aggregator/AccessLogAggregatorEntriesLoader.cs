using AccessTracker.Domain.AccessLog;
using AccessTracker.Domain.AccessLog.Aggregations.Common.Checkpoints;

namespace AccessTracker.Application.AccessLog.Aggregations.Common.Services.Aggregator;


public class AccessLogAggregatorEntriesLoader<TAggregation> :
    IAccessLogAggregatorEntriesLoader<TAggregation>
{
    private readonly IAccessLogAggregationTypeMapper _accessLogAggregationTypeMapper;
    private readonly IAccessLogAggregationCheckpointRepository _aggregationCheckpointRepository;
    private readonly IAccessLogRepository _accessLogRepository;
    
    public AccessLogAggregatorEntriesLoader(
        IAccessLogAggregationTypeMapper accessLogAggregationTypeMapper,
        IAccessLogAggregationCheckpointRepository aggregationCheckpointRepository,
        IAccessLogRepository accessLogRepository)
    {
        _accessLogAggregationTypeMapper = accessLogAggregationTypeMapper;
        _aggregationCheckpointRepository = aggregationCheckpointRepository;
        _accessLogRepository = accessLogRepository;
    }

    
    public async Task<IReadOnlyList<AccessLogEntry>> LoadEntriesAsync(
        int entriesCount,
        CancellationToken cancellationToken)
    {
        var aggregationType = _accessLogAggregationTypeMapper
            .GetAggregationType<TAggregation>();

        var lastAggregatedEventId = await _aggregationCheckpointRepository
            .GetLastAggregatedEventIdAsync(aggregationType, cancellationToken);

        return await _accessLogRepository.GetWithIdGreaterThanAsync(
            lastAggregatedEventId ?? 0,
            entriesCount,
            cancellationToken);
    }
}