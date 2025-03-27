using AccessTracker.Domain.AccessLog;
using AccessTracker.Domain.AccessLog.Aggregations.LastAccess;
using AccessTracker.Domain.AccessLog.Aggregations.UserIpLink;
using ErrorOr;
using Microsoft.Extensions.Logging;

namespace AccessTracker.Application.AccessLog;


public class AccessLogService :
    IUserIpListProvider,
    IUserLastAccessProvider,
    IUserSearchService
{
    private readonly IAccessLogLastAccessAccessLogAggregationRepository _lastAccessAccessLogAggregationRepository;
    private readonly IAccessLogUserIpLinkAccessLogAggregationRepository _userIpLinkAccessLogAggregationRepository;
    private readonly ILogger<AccessLogService> _logger;


    public AccessLogService(
        IAccessLogLastAccessAccessLogAggregationRepository lastAccessAccessLogAggregationRepository,
        IAccessLogUserIpLinkAccessLogAggregationRepository userIpLinkAccessLogAggregationRepository,
        ILogger<AccessLogService> logger)
    {
        _lastAccessAccessLogAggregationRepository = lastAccessAccessLogAggregationRepository;
        _userIpLinkAccessLogAggregationRepository = userIpLinkAccessLogAggregationRepository;
        _logger = logger;
    }

    
    public async Task<ErrorOr<UserIpListResult>> GetUserIpListAsync(
        UserIpListQuery query,
        CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogTrace(
                "Retrieving IP list for user {UserId} (skip: {Skip}, take: {Take}).",
                query.UserId,
                query.Skip,
                query.Take);
            
            var (totalCount, ipList) = await _userIpLinkAccessLogAggregationRepository
                .GetUserAccessedFromIpListAsync(
                    query.UserId,
                    query.Skip,
                    query.Take,
                    cancellationToken);
            
            _logger.LogTrace(
                "Retrieved {Count} IP addresses for user {UserId} (total: {TotalCount}).",
                ipList.Count,
                query.UserId,
                totalCount);

            return new UserIpListResult(query.UserId, totalCount, ipList);
        }
        catch (Exception exception)
        {
            _logger.LogError(
                exception,
                "An error occurred while retrieving IP list for user {UserId}: {ErrorMessage}",
                query.UserId,
                exception.Message);
            
            return Error.Unexpected(
                code: "Error.AccessTracker.AccessLog.GetUserIpList.Unexpected",
                description: exception.ToString());
        }
    }

    
    public async Task<ErrorOr<UserAccess>> GetLastAccessAsync(
        long userId,
        CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogTrace(
                "Retrieving last access data for user {UserId}.",
                userId);
            
            var lastAccessOrNull = await _lastAccessAccessLogAggregationRepository
                .GetByUserIdAsync(
                    userId,
                    cancellationToken);

            if (lastAccessOrNull is not {} lastAccess)
            {
                return Error.NotFound(
                    code: "Error.AccessTracker.AccessLog.GetUserLastAccess.NotFound",
                    description: $"Record for user {userId} not found");
            }
            
            _logger.LogTrace(
                "Retrieved last access data for user {UserId}: {AccessTime}, IP address: {IpAddress}",
                userId,
                lastAccess.AccessUtcTime,
                lastAccess.IpAddress);

            return lastAccess;
        }
        catch (Exception exception)
        {
            _logger.LogError(
                exception,
                "An error occurred while retrieving IP list for user {UserId}: {ErrorMessage}",
                userId,
                exception.Message);
            
            return Error.Unexpected(
                code: "Error.AccessTracker.AccessLog.GetUserLastAccess.Unexpected",
                description: exception.ToString());
        }
    }

    
    public async Task<ErrorOr<UserSearchResult>> SearchUsersAsync(
        UserSearchQuery query,
        CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogTrace(
                "Searching users. IpAddress: {IpAddress}",
                query.IpAddress);

            var (totalCount, users) = await _userIpLinkAccessLogAggregationRepository
                .SearchUsersByIpAddressAsync(
                    query.IpAddress,
                    query.Skip,
                    query.Take,
                     cancellationToken);
            
            _logger.LogTrace(
                "Retrieved {Count} users. Total: {TotalCount}",
                users.Count,
                totalCount);

            return new UserSearchResult(query, totalCount, users);
        }
        catch (Exception exception)
        {
            _logger.LogError(
                exception,
                "An error occurred while searching users: {ErrorMessage}",
                exception.Message);
            
            return Error.Unexpected(
                code: "Error.AccessTracker.AccessLog.SearchUsers.Unexpected",
                description: exception.ToString());
        }
    }
}