namespace AccessTracker.Application.AccessLog.Aggregations.Common.Services.Aggregator;

public class AccessLogAggregatorOptions<TAggregation>
{
    public int LogEntriesBatchSize { get; set; }

    public TimeSpan AggregationInterval { get; set; }
}