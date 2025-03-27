using Microsoft.Extensions.Logging;

namespace AccessTracker.Application.EventTracking.Services;

public class AccessEventTrackFailedHandler : IAccessEventTrackFailedHandler
{
    private readonly ILogger<AccessEventTrackFailedHandler> _logger;

    public AccessEventTrackFailedHandler(
        ILogger<AccessEventTrackFailedHandler> logger)
    {
        _logger = logger;
    }


    public ValueTask HandleTrackFailureAsync(
        Exception exception,
        long userId,
        string ipAddress,
        DateTimeOffset registrationTime,
        CancellationToken cancellationToken)
    {
        _logger.LogError(
            exception,
            "Failed to save access event user: {UserId}, IP address: {IpAddress}, timestamp: {Timestamp}",
            userId,
            ipAddress,
            registrationTime);

        return ValueTask.CompletedTask;
    }
}