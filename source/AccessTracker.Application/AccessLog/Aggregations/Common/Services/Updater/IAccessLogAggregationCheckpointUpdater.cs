using AccessTracker.Data;
using AccessTracker.Domain.AccessLog.Aggregations;

namespace AccessTracker.Application.AccessLog.Aggregations.Common.Services.Updater;


public interface IAccessLogAggregationCheckpointUpdater
{
    Task UpdateAggregationCheckpointAsync(
        AccessTrackerDbContext dbContext,
        AccessLogAggregationType aggregationType,
        long lastAggregatedEntryId,
        CancellationToken cancellationToken);
}