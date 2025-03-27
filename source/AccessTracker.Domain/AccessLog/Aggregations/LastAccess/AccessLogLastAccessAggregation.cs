namespace AccessTracker.Domain.AccessLog.Aggregations.LastAccess;

public class AccessLogLastAccessAggregation
{
    public long UserId { get; init; }
    
    public DateTimeOffset LastAccessUtcTime { get; init; }

    public string LastIpAddress { get; init; }
}