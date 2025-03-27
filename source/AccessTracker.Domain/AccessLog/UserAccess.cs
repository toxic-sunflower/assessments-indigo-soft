namespace AccessTracker.Domain.AccessLog;

public readonly record struct UserAccess(
    long UserId,
    DateTimeOffset AccessUtcTime,
    string IpAddress);