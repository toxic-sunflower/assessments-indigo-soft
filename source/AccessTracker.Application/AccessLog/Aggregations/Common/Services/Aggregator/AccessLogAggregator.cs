using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AccessTracker.Application.AccessLog.Aggregations.Common.Services.Aggregator;


public class AccessLogAggregator<TAggregation> : IAccessLogAggregator<TAggregation>
{
    private readonly IAccessLogAggregatorEntriesLoader<TAggregation> _accessLogAggregatorEntriesLoader;
    private readonly IAccessLogAggregationUpdater<TAggregation> _aggregationUpdater;
    private readonly IOptions<AccessLogAggregatorOptions<TAggregation>> _options;
    private readonly ILogger _logger;

    private volatile int _isRunning;


    public AccessLogAggregator(
        IAccessLogAggregatorEntriesLoader<TAggregation> accessLogAggregatorEntriesLoader,
        IAccessLogAggregationUpdater<TAggregation> aggregationUpdater,
        IOptions<AccessLogAggregatorOptions<TAggregation>> options,
        ILoggerFactory loggerFactory)
    {
        _accessLogAggregatorEntriesLoader = accessLogAggregatorEntriesLoader;
        _aggregationUpdater = aggregationUpdater;
        _options = options;
        _logger = loggerFactory.CreateLogger(GetType());
    }

    public TimeSpan GetInterval() => _options.Value.AggregationInterval;

    
    public async Task AggregateAsync(CancellationToken cancellationToken)
    {
        if (Interlocked.CompareExchange(ref _isRunning, 1, 0) == 0)
        {
            try
            {
                var entries = await _accessLogAggregatorEntriesLoader.LoadEntriesAsync(
                    _options.Value.LogEntriesBatchSize,
                    cancellationToken);

                await _aggregationUpdater.UpdateAggregationAsync(entries, cancellationToken);

                return;
            }
            catch (Exception exception)
            {
                _logger.LogError(
                    exception,
                    "Error occured executing aggregator {Aggregator}: {ErrorMessage}",
                    GetType().Name,
                    exception.Message);

                throw;
            }
            finally
            {
                Interlocked.Exchange(ref _isRunning, 0);
            }
        }
        
        _logger.LogCritical(
            "Previous aggregation for {AggregationType} is still running. Aborting.",
            typeof(TAggregation).Name);
    }
}