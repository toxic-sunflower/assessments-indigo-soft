using AccessTracker.Application.AccessLog.Aggregations.Common.Services;
using AccessTracker.Application.AccessLog.Aggregations.Common.Services.Updater;
using AccessTracker.Application.AccessLog.Aggregations.UserIpLinks.Services;
using AccessTracker.Data;
using AccessTracker.Data.Storages;
using AccessTracker.Domain.AccessLog;
using AccessTracker.Domain.AccessLog.Aggregations.UserIpLink;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AccessTracker.Application.AccessLog.Aggregations.UserIpLinks;


public class AccessLogUserIpLinksAggregationUpdater :
    AccessLogAggregationUpdaterBase<AccessLogUserIpLinkAggregation>
{
    private IAccessLogUserIpLinkAggregationMapper _aggregationMapper;
    
    public AccessLogUserIpLinksAggregationUpdater(
        IDbContextFactory<AccessTrackerDbContext> dbContextFactory,
        IAccessLogAggregationTypeMapper accessLogAggregationTypeMapper,
        IAccessLogAggregationCheckpointUpdater checkpointUpdater,
        ILoggerFactory loggerFactory,
        IAccessLogUserIpLinkAggregationMapper aggregationMapper,
        IStorage storage) :
            base(dbContextFactory, accessLogAggregationTypeMapper, checkpointUpdater, loggerFactory, storage)
    {
        _aggregationMapper = aggregationMapper;
    }

    
    protected override async Task UpdateAggregationAsync(IReadOnlyList<AccessLogEntry> accessLogEntries,
        AccessTrackerDbContext dbContext,
        IStorage storage,
        ILogger logger,
        CancellationToken cancellationToken)
    {
        var aggregations = accessLogEntries
            .GroupBy(x => x.UserId)
            .SelectMany(x => _aggregationMapper.MapLastAccessAggregation(
                x.Key,
                x.Select(i => i.IpAddress)
                    .Distinct(StringComparer.Ordinal)
                    .ToList()))
            .ToList();
        
        await dbContext.AccessLogUserIpLinkAggregations
            .UpsertRange(aggregations)
            .On(x => x.UserId)
            .NoUpdate()
            .RunAsync(cancellationToken);
    }
}