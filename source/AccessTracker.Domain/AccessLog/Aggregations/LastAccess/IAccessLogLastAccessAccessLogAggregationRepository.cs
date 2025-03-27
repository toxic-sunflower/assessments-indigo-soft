namespace AccessTracker.Domain.AccessLog.Aggregations.LastAccess;

public interface IAccessLogLastAccessAccessLogAggregationRepository
{
    Task<UserAccess?> GetByUserIdAsync(
        long userId,
        CancellationToken cancellationToken);
}