using AccessTracker.Domain.AccessLog.Aggregations.UserIpLink;
using Microsoft.EntityFrameworkCore;

namespace AccessTracker.Data.Repositories;


public class AccessLogUserIpLinkAccessLogAggregationRepository :
    IAccessLogUserIpLinkAccessLogAggregationRepository
{
    private readonly IDbContextFactory<AccessTrackerDbContext> _dbContextFactory;

    public AccessLogUserIpLinkAccessLogAggregationRepository(
        IDbContextFactory<AccessTrackerDbContext> dbContextFactory) =>
            _dbContextFactory = dbContextFactory;


    public async Task<(long totalCount, IReadOnlyCollection<string> items)> GetUserAccessedFromIpListAsync(
        long userId,
        int skip,
        int take,
        CancellationToken cancellationToken)
    {
        await using var dbContext = await _dbContextFactory.CreateDbContextAsync(cancellationToken);

        var query = dbContext.AccessLogUserIpLinkAggregations
            .Where(x => x.UserId == userId);
        
        var items = await query
            .OrderBy(x => x.IpAddress)
            .Select(x => x.IpAddress)
            .Skip(skip)
            .Take(take)
            .ToListAsync(cancellationToken);
        
        var count = await query.CountAsync(cancellationToken);
        
        return (count, items);
    }

    public async Task<(long totalCount, IReadOnlyCollection<long> items)> SearchUsersByIpAddressAsync(
        string ipAddress,
        int skip,
        int take,
        CancellationToken cancellationToken)
    {
        await using var dbContext = await _dbContextFactory.CreateDbContextAsync(cancellationToken);

        var query = dbContext.AccessLogUserIpLinkAggregations
            .Where(x => x.IpAddress.StartsWith(ipAddress));
        
        var items = await query
            .OrderBy(x => x.IpAddress)
            .Select(x => x.UserId)
            .Skip(skip)
            .Take(take)
            .ToListAsync(cancellationToken);
        
        var count = await query.CountAsync(cancellationToken);
        
        return (count, items);
    }
}