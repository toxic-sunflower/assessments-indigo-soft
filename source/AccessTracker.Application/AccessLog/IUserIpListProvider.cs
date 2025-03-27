using ErrorOr;

namespace AccessTracker.Application.AccessLog;


public readonly record struct UserIpListQuery(
    long UserId,
    int Skip,
    int Take);

public readonly record struct UserIpListResult(
    long UserId,
    long TotalCount,
    IReadOnlyCollection<string> Addresses);

public interface IUserIpListProvider
{
    Task<ErrorOr<UserIpListResult>> GetUserIpListAsync(
        UserIpListQuery query,
        CancellationToken cancellationToken = default);
}