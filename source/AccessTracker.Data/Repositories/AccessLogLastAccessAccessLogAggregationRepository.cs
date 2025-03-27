using AccessTracker.Domain.AccessLog;
using AccessTracker.Domain.AccessLog.Aggregations.LastAccess;
using Microsoft.EntityFrameworkCore;

namespace AccessTracker.Data.Repositories;


public class AccessLogLastAccessAccessLogAggregationRepository :
    IAccessLogLastAccessAccessLogAggregationRepository
{
    private readonly IDbContextFactory<AccessTrackerDbContext> _dbContextFactory;

    public AccessLogLastAccessAccessLogAggregationRepository(
        IDbContextFactory<AccessTrackerDbContext> dbContextFactory) =>
            _dbContextFactory = dbContextFactory;

    
    public async Task<UserAccess?> GetByUserIdAsync(
        long userId,
        CancellationToken cancellationToken)
    {
        await using var context = await _dbContextFactory.CreateDbContextAsync(cancellationToken);

        return await context.AccessLog
            .Where(x => x.UserId == userId)
            .Select(x =>
                new UserAccess(
                    x.UserId,
                    x.Timestamp,
                    x.IpAddress))
            .SingleOrDefaultAsync(cancellationToken);
    }
}