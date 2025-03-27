using AccessTracker.Domain.AccessLog;
using AccessTracker.Domain.AccessLog.Aggregations.LastAccess;
using Microsoft.EntityFrameworkCore;

namespace AccessTracker.Data.Repositories;


public class AccessLogLastAccessAggregationRepository :
    IAccessLogLastAccessAggregationRepository
{
    private readonly IDbContextFactory<AccessTrackerDbContext> _dbContextFactory;

    public AccessLogLastAccessAggregationRepository(
        IDbContextFactory<AccessTrackerDbContext> dbContextFactory) =>
            _dbContextFactory = dbContextFactory;

    
    public async Task<UserAccess?> GetByUserIdAsync(
        long userId,
        CancellationToken cancellationToken)
    {
        await using var context = await _dbContextFactory.CreateDbContextAsync(cancellationToken);

        return await context.AccessLogLastAccessAggregations
            .Where(x => x.UserId == userId)
            .Select(x =>
                new UserAccess(
                    x.UserId,
                    x.LastAccessUtcTime,
                    x.LastIpAddress))
            .SingleOrDefaultAsync(cancellationToken);
    }
}