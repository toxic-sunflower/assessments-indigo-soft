using AccessTracker.Data;
using AccessTracker.Domain.AccessLog.Aggregations;
using AccessTracker.Domain.AccessLog.Aggregations.Common.Checkpoints;
using Microsoft.EntityFrameworkCore;

namespace AccessTracker.Application.AccessLog.Aggregations.Common.Services.Updater;


public class AccessLogAggregationCheckpointUpdater : IAccessLogAggregationCheckpointUpdater
{
    public async Task UpdateAggregationCheckpointAsync(
        AccessTrackerDbContext dbContext,
        AccessLogAggregationType aggregationType,
        long lastAggregatedEntryId,
        CancellationToken cancellationToken)
    {
        await dbContext.AccessLogAggregationCheckpoints
            .Upsert(
                new AccessLogAggregationCheckpoint
                {
                    AggregationType = aggregationType,
                    LastAggregatedEventId = lastAggregatedEntryId
                })
            .On(x => x.AggregationType)
            .WhenMatched(
                x => new AccessLogAggregationCheckpoint
                {
                    LastAggregatedEventId = x.LastAggregatedEventId
                })
            .RunAsync(cancellationToken);
    }
}