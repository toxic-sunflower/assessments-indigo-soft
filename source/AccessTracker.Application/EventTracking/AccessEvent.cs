namespace AccessTracker.Application.EventTracking;

public readonly record struct AccessEvent(
    long UserId,
    string IpAddress);