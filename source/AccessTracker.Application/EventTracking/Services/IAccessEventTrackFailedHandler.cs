namespace AccessTracker.Application.EventTracking.Services;

public interface IAccessEventTrackFailedHandler
{
    ValueTask HandleTrackFailureAsync(Exception exception, long userId,
        string ipAddress,
        DateTimeOffset registrationTime,
        CancellationToken cancellationToken);
}