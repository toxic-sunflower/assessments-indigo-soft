namespace AccessTracker.Domain.AccessLog.Aggregations.Common.Checkpoints;

public interface IAccessLogAggregationCheckpointRepository
{
    Task<long?> GetLastAggregatedEventIdAsync(
        AccessLogAggregationType aggregationType,
        CancellationToken cancellationToken);
}