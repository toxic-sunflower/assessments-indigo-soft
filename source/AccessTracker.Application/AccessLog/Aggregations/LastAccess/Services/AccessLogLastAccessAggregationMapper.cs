using AccessTracker.Domain.AccessLog;
using AccessTracker.Domain.AccessLog.Aggregations.LastAccess;

namespace AccessTracker.Application.AccessLog.Aggregations.LastAccess.Services;

public class AccessLogLastAccessAggregationMapper : IAccessLogLastAccessAggregationMapper
{
    public AccessLogLastAccessAggregation MapLastAccessAggregation(
        AccessLogEntry accessLogEntry)
    {
        return new AccessLogLastAccessAggregation
        {
            UserId = accessLogEntry.UserId,
            LastAccessUtcTime = accessLogEntry.Timestamp,
            LastIpAddress = accessLogEntry.IpAddress
        };
    }
}