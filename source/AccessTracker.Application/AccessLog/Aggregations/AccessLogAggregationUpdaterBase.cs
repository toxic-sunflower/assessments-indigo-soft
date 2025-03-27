using AccessTracker.Application.AccessLog.Aggregations.Common.Services;
using AccessTracker.Application.AccessLog.Aggregations.Common.Services.Updater;
using AccessTracker.Data;
using AccessTracker.Data.Storages;
using AccessTracker.Domain.AccessLog;
using AccessTracker.Domain.AccessLog.Aggregations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AccessTracker.Application.AccessLog.Aggregations;


public abstract class AccessLogAggregationUpdaterBase<TAggregation> :
    IAccessLogAggregationUpdater<TAggregation>
{
    private readonly IDbContextFactory<AccessTrackerDbContext> _dbContextFactory;
    private readonly IAccessLogAggregationTypeMapper _accessLogAggregationTypeMapper;
    private readonly IAccessLogAggregationCheckpointUpdater _aggregationCheckpointUpdater;
    private readonly IStorage _storage;
    private readonly ILogger _logger;

    
    protected AccessLogAggregationUpdaterBase(
        IDbContextFactory<AccessTrackerDbContext> dbContextFactory,
        IAccessLogAggregationTypeMapper accessLogAggregationTypeMapper,
        IAccessLogAggregationCheckpointUpdater aggregationCheckpointUpdater,
        ILoggerFactory loggerFactory,
        IStorage storage)
    {
        _dbContextFactory = dbContextFactory;
        _accessLogAggregationTypeMapper = accessLogAggregationTypeMapper;
        _storage = storage;
        _aggregationCheckpointUpdater = aggregationCheckpointUpdater;
        _logger = loggerFactory.CreateLogger(GetType());
    }
    
    
    protected AccessLogAggregationType AggregationType =>
        _accessLogAggregationTypeMapper.GetAggregationType<TAggregation>();
    
    
    public async Task UpdateAggregationAsync(
        IReadOnlyList<AccessLogEntry> accessLogEntries,
        CancellationToken cancellationToken)
    {
        var aggregationType = _accessLogAggregationTypeMapper.GetAggregationType<TAggregation>();
        
        await using var dbContext = await _dbContextFactory.CreateDbContextAsync(cancellationToken);
        await using var tx = await _storage.BeginTransactionAsync(dbContext, cancellationToken);
                
        await UpdateAggregationAsync(
            accessLogEntries,
            dbContext,
            _storage,
            _logger,
            cancellationToken);
        
        var lastAggregatedEventId = accessLogEntries
            .Select(x => x.Id)
            .OrderByDescending(x => x)
            .FirstOrDefault();

        await _aggregationCheckpointUpdater.UpdateAggregationCheckpointAsync(
            dbContext,
            aggregationType,
            lastAggregatedEventId,
            cancellationToken);
      
        await tx.CommitAsync(cancellationToken);
    }

    protected abstract Task UpdateAggregationAsync(IReadOnlyList<AccessLogEntry> accessLogEntries,
        AccessTrackerDbContext dbContext,
        IStorage storage,
        ILogger logger,
        CancellationToken cancellationToken);
    
}