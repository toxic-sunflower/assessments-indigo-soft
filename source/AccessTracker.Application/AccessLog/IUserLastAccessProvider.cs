using AccessTracker.Domain.AccessLog;
using ErrorOr;

namespace AccessTracker.Application.AccessLog;

public interface IUserLastAccessProvider
{
    Task<ErrorOr<UserAccess>> GetLastAccessAsync(
        long userId,
        CancellationToken cancellationToken);
}