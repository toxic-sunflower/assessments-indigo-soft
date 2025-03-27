using AccessTracker.Domain.AccessLog.Aggregations.UserIpLink;

namespace AccessTracker.Application.AccessLog.Aggregations.UserIpLinks.Services;

public interface IAccessLogUserIpLinkAggregationMapper
{
    IReadOnlyList<AccessLogUserIpLinkAggregation
    > MapLastAccessAggregation(
        long userId,
        IEnumerable<string> ipAddresses);
}