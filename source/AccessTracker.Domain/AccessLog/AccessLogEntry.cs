namespace AccessTracker.Domain.AccessLog;


public class AccessLogEntry
{
    public long Id { get; init; }
    
    public required long UserId { get; init; }
    
    public required string IpAddress { get; init; } = default!;

    public DateTimeOffset Timestamp { get; init; } = DateTimeOffset.UtcNow;
}
