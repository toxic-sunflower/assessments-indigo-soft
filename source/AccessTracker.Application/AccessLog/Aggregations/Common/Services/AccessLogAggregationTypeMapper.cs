using AccessTracker.Domain.AccessLog.Aggregations;
using AccessTracker.Domain.AccessLog.Aggregations.LastAccess;
using AccessTracker.Domain.AccessLog.Aggregations.UserIpLink;

namespace AccessTracker.Application.AccessLog.Aggregations.Common.Services;

public class AccessLogAggregationTypeMapper : IAccessLogAggregationTypeMapper
{
    public AccessLogAggregationType GetAggregationType<TAggregation>() =>
        GetAggregationType(typeof(TAggregation));

    public AccessLogAggregationType GetAggregationType(Type aggregationType)
    {
        return aggregationType == typeof(AccessLogLastAccessAggregation)
            ? AccessLogAggregationType.LastAccess
            : aggregationType == typeof(AccessLogUserIpLinkAggregation)
                ? AccessLogAggregationType.UserIpLink
                : throw new ArgumentOutOfRangeException($"Unsupported aggregation type: {aggregationType.Name}");
    }

    public Type GetEntityType(AccessLogAggregationType aggregationType)
    {
        return aggregationType switch
        {
            AccessLogAggregationType.LastAccess => typeof(AccessLogLastAccessAggregation),
            AccessLogAggregationType.UserIpLink => typeof(AccessLogUserIpLinkAggregation),
            _ => throw new ArgumentOutOfRangeException(nameof(aggregationType), aggregationType, null)
        };
    }
}