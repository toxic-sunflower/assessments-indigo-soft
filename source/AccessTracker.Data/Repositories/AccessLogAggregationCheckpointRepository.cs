using AccessTracker.Domain.AccessLog.Aggregations;
using AccessTracker.Domain.AccessLog.Aggregations.Common.Checkpoints;
using Microsoft.EntityFrameworkCore;

namespace AccessTracker.Data.Repositories;

public class AccessLogAggregationCheckpointRepository :
    IAccessLogAggregationCheckpointRepository
{
    private readonly IDbContextFactory<AccessTrackerDbContext> _dbContextFactory;

    public AccessLogAggregationCheckpointRepository(
        IDbContextFactory<AccessTrackerDbContext> dbContextFactory) =>
            _dbContextFactory = dbContextFactory;


    public async Task<long?> GetLastAggregatedEventIdAsync(
        AccessLogAggregationType aggregationType,
        CancellationToken cancellationToken)
    {
        await using var dbContext = await _dbContextFactory.CreateDbContextAsync(cancellationToken);

        return await dbContext.AccessLogAggregationCheckpoints
            .Where(x => x.AggregationType == aggregationType)
            .Select(x => x.LastAggregatedEventId)
            .SingleOrDefaultAsync(cancellationToken);
    }
}