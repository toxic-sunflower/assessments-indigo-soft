namespace AccessTracker.Domain.AccessLog.Aggregations.UserIpLink;

public interface IAccessLogUserIpLinkAccessLogAggregationRepository
{
    Task<(long totalCount, IReadOnlyCollection<string> items)> GetUserAccessedFromIpListAsync(
        long userId,
        int skip,
        int take,
        CancellationToken cancellationToken);

    Task<(long totalCount, IReadOnlyCollection<long> items)> SearchUsersByIpAddressAsync(
        string ipAddress,
        int skip,
        int take,
        CancellationToken cancellationToken);
}