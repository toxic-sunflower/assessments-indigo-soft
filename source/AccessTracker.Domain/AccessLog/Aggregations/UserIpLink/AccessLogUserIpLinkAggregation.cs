namespace AccessTracker.Domain.AccessLog.Aggregations.UserIpLink;

public class AccessLogUserIpLinkAggregation
{
    public long UserId { get; init; }
    
    public string IpAddress { get; init; }
}