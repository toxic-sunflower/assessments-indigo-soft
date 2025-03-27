namespace AccessTracker.Application;

public class ApplicationSettings
{
    public int AccessLogAggregationEntriesBatchSize { get; init; }
    
    public TimeSpan AccessLogAggregationInterval { get; init; }
    
    public int EventTrackingAttemptsCount { get; init; }
    
    public TimeSpan EventTrackingRetryInterval { get; init; }
}