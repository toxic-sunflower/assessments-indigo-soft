using AccessTracker.Domain.AccessLog;

namespace AccessTracker.Application.AccessLog.Aggregations;

public interface IAccessLogAggregationUpdater<TAggregation>
{
    Task UpdateAggregationAsync(
        IReadOnlyList<AccessLogEntry> logEntries,
        CancellationToken cancellationToken);
}