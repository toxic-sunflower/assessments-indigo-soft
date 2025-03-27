namespace AccessTracker.Domain.AccessLog.Aggregations.Common.Services;

public interface IAccessLogAggregationUpdater<TAggregation>
{
    ValueTask<IReadOnlyCollection<TAggregation>> UpdateAggregationsAsync(
        IReadOnlyCollection<AccessLogEntry> logEntries,
        CancellationToken cancellationToken);
}