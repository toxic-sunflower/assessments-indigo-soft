using AccessTracker.Application.AccessLog.Aggregations.Common.Services;
using AccessTracker.Application.AccessLog.Aggregations.Common.Services.Updater;
using AccessTracker.Application.AccessLog.Aggregations.LastAccess.Services;
using AccessTracker.Data;
using AccessTracker.Data.Storages;
using AccessTracker.Domain.AccessLog;
using AccessTracker.Domain.AccessLog.Aggregations.LastAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AccessTracker.Application.AccessLog.Aggregations.LastAccess;


public class AccessLogLastAccessAggregationUpdater : 
    AccessLogAggregationUpdaterBase<AccessLogLastAccessAggregation>
{
    private readonly IAccessLogLastAccessAggregationMapper _accessLogLastAccessAggregationMapper;
    
    public AccessLogLastAccessAggregationUpdater(
        IDbContextFactory<AccessTrackerDbContext> dbContextFactory,
        IAccessLogAggregationTypeMapper accessLogAggregationTypeMapper,
        IAccessLogAggregationCheckpointUpdater checkpointUpdater,
        ILoggerFactory loggerFactory,
        IAccessLogLastAccessAggregationMapper accessLogLastAccessAggregationMapper,
        IStorage storage) : 
            base(dbContextFactory, accessLogAggregationTypeMapper, checkpointUpdater, loggerFactory, storage)
    {
        _accessLogLastAccessAggregationMapper = accessLogLastAccessAggregationMapper;
    }


    protected override async Task UpdateAggregationAsync(
        IReadOnlyList<AccessLogEntry> accessLogEntries,
        AccessTrackerDbContext dbContext,
        IStorage storage,
        ILogger logger,
        CancellationToken cancellationToken)
    {
        logger.LogTrace("Grouping last access events by users");

        var aggregations = accessLogEntries
            .GroupBy(x => x.UserId)
            .Select(x => _accessLogLastAccessAggregationMapper
                .MapLastAccessAggregation(
                    x.OrderByDescending(x => x.Id).First()))
            .ToList();

        logger.LogTrace("{UsersCount} user last access aggregations will be updated", aggregations.Count);
        
        await dbContext.AccessLogLastAccessAggregations
            .UpsertRange(aggregations)
            .On(x => new {x.UserId})
            .WhenMatched(
                (existing, incoming) => new AccessLogLastAccessAggregation
                {
                    LastIpAddress = incoming.LastIpAddress,
                    LastAccessUtcTime = incoming.LastAccessUtcTime
                })
            .RunAsync(cancellationToken);
    }
}