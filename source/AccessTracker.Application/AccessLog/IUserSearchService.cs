using ErrorOr;

namespace AccessTracker.Application.AccessLog;

public readonly record struct UserSearchQuery(
    string IpAddress,
    int Skip,
    int Take);

public readonly record struct UserSearchResult(
    UserSearchQuery Query,
    long TotalCount,
    IReadOnlyCollection<long> UserIds);

public interface IUserSearchService
{
    Task<ErrorOr<UserSearchResult>> SearchUsersAsync(
        UserSearchQuery query,
        CancellationToken cancellationToken);
}