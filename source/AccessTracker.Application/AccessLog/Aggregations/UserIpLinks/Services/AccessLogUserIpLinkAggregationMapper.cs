using AccessTracker.Domain.AccessLog.Aggregations.UserIpLink;

namespace AccessTracker.Application.AccessLog.Aggregations.UserIpLinks.Services;

public class AccessLogUserIpLinkAggregationMapper : IAccessLogUserIpLinkAggregationMapper
{
    public IReadOnlyList<AccessLogUserIpLinkAggregation> MapLastAccessAggregation(
        long userId,
        IEnumerable<string> ipAddresses)
    {
        return ipAddresses
            .Select(
                x => new AccessLogUserIpLinkAggregation
                {
                    UserId = userId,
                    IpAddress = x
                })
            .ToList();
    }
}