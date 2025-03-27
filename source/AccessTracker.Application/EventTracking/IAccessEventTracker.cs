using ErrorOr;

namespace AccessTracker.Application.EventTracking;

public interface IAccessEventTracker
{
    Task<ErrorOr<Success>> TrackAccessEventAsync(
        AccessEvent evt,
        CancellationToken cancellationToken);
}