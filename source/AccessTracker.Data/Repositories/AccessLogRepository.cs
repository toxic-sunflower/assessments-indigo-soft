using AccessTracker.Domain.AccessLog;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AccessTracker.Data.Repositories;


public class AccessLogRepository : IAccessLogRepository
{
    private readonly IDbContextFactory<AccessTrackerDbContext> _dbContextFactory;
    private readonly ILogger<AccessLogRepository> _logger;
    
    public AccessLogRepository(
        IDbContextFactory<AccessTrackerDbContext> dbContextFactory,
        ILogger<AccessLogRepository> logger)
    {
        _dbContextFactory = dbContextFactory;
        _logger = logger;
    }

    
    public async Task AddEntryAsync(
        AccessLogEntry entry,
        CancellationToken cancellationToken)
    {
        await using var db = await _dbContextFactory
            .CreateDbContextAsync(cancellationToken);

        await db.AccessLog.AddAsync(entry, cancellationToken);

        await db.SaveChangesAsync(cancellationToken);

        _logger.LogTrace(
            "Access event saved: {UserId} {Ip} {Timestamp}",
            entry.UserId,
            entry.IpAddress,
            entry.Timestamp);
    }

    public async Task<IReadOnlyList<AccessLogEntry>> GetWithIdGreaterThanAsync(
        long id,
        int count,
        CancellationToken cancellationToken)
    {
        await using var dbContext = await _dbContextFactory.CreateDbContextAsync(cancellationToken);
        
        return await dbContext.AccessLog
            .Where(x => x.Id > id)
            .OrderBy(x => x.Id)
            .Take(count)
            .ToListAsync(cancellationToken);
    }
}