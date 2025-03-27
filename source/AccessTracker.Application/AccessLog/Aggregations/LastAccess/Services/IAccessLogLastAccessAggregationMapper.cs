using AccessTracker.Domain.AccessLog;
using AccessTracker.Domain.AccessLog.Aggregations.LastAccess;

namespace AccessTracker.Application.AccessLog.Aggregations.LastAccess.Services;

public interface IAccessLogLastAccessAggregationMapper
{
    AccessLogLastAccessAggregation MapLastAccessAggregation(AccessLogEntry accessLogEntry);
}