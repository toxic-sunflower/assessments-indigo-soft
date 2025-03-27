using AccessTracker.Application.AccessLog.Aggregations.Common.Services.Aggregator;

namespace AccessTracker.Services;


public class AggregationService : BackgroundService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    
    public AggregationService(IServiceScopeFactory serviceScopeFactory) => 
        _serviceScopeFactory = serviceScopeFactory;


    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await using var scope = _serviceScopeFactory.CreateAsyncScope();
        
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<AggregationService>>();
        var aggregators = scope.ServiceProvider.GetServices<IAccessLogAggregator>();

        try
        {
            var workers = GetWorkers(aggregators.ToList(), logger, stoppingToken);
            await Task.WhenAll(workers);
        }
        catch (TaskCanceledException)
        {
            
        }
        catch (Exception exception)
        {
            logger.LogError(
                exception,
                "Error executing aggregation service: {ErrorMessage}",
                exception.Message);
        }
    }

    private IReadOnlyList<Task> GetWorkers(
        IReadOnlyCollection<IAccessLogAggregator> aggregators,
        ILogger<AggregationService> logger,
        CancellationToken stoppingToken)
    {
        logger.LogTrace("Creating workers for {AggregationsCount} aggregations", aggregators.Count);
        
        return aggregators
            .Select(aggregator => WorkerLoop(aggregator, logger, stoppingToken))
            .ToList();
    }

    private async Task WorkerLoop(
        IAccessLogAggregator aggregator,
        ILogger<AggregationService> logger,
        CancellationToken stoppingToken)
    {
        logger.LogTrace(
            "Starting worker loop for {AggregatorType} aggregation",
            aggregator.GetType().Name);
        
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await aggregator.AggregateAsync(stoppingToken);
            }
            catch (Exception exception)
            {
                logger.LogError(
                    exception,
                    "Error executing aggregator for {AggregatorType} aggregation: {ErrorMessage}",
                    aggregator.GetType().Name,
                    exception.Message);
            }

            await Task.Delay(aggregator.GetInterval(), stoppingToken);
        }
        
        logger.LogTrace(
            "Stopping worker loop for {AggregatorType} aggregation",
            aggregator.GetType().Name);
    }
}