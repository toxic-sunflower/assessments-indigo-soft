namespace AccessTracker.Domain.AccessLog;

public interface IAccessLogRepository
{
    Task AddEntryAsync(
        AccessLogEntry entry,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<AccessLogEntry>> GetWithIdGreaterThanAsync(
        long id,
        int count,
        CancellationToken cancellationToken);
}