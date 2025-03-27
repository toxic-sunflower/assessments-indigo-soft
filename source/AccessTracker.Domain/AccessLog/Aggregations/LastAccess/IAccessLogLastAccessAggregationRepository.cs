namespace AccessTracker.Domain.AccessLog.Aggregations.LastAccess;

public interface IAccessLogLastAccessAggregationRepository
{
    Task<UserAccess?> GetByUserIdAsync(
        long userId,
        CancellationToken cancellationToken);
}