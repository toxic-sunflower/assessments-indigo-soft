namespace AccessTracker.Domain.AccessLog.Aggregations.Common.Checkpoints;


public class AccessLogAggregationCheckpoint
{
    public AccessLogAggregationType AggregationType { get; init; }
    
    public long LastAggregatedEventId { get; set; }
}