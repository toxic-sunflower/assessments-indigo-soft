using AccessTracker.Domain.AccessLog;

namespace AccessTracker.Application.AccessLog.Aggregations.Common.Services.Aggregator;

public interface IAccessLogAggregatorEntriesLoader<TAggregation>
{
    Task<IReadOnlyList<AccessLogEntry>> LoadEntriesAsync(int entriesCount, CancellationToken cancellationToken);
}