using AccessTracker.Application.AccessLog;
using AccessTracker.Domain.AccessLog;
using AccessTracker.Helpers;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace AccessTracker.Modules.AccessLog;


public class AccessLogApiModule : IMinimalApiModule
{
    // It should be the config option in the real world.
    private const int DefaultPageSize = 50;
    
    
    public void RegisterEndpoints(IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("access-log/user").WithTags("access-log");
            
        group.MapGet("search", SearchUsersByIpAsync);
        group.MapGet("{userId:long}/access-ip-addresses", GetUserIpListAsync);
        group.MapGet("{userId:long}/last-access", GetUserLastAccessAsync);
    }


    private static async Task<Results<Ok<UserSearchResult>, ProblemHttpResult>> SearchUsersByIpAsync(
        [FromQuery] string ipAddress,
        [FromQuery] int? page,
        [FromQuery] int? pageSize,
        [FromServices] PagingTransformer pagingTransformer,
        [FromServices] IUserSearchService service,
        CancellationToken cancellationToken)
    {
        var (skip, take) = pagingTransformer.GetSkipTake(page, pageSize, DefaultPageSize);
        
        var query = new UserSearchQuery(
            ipAddress,
            skip,
            take);

        var searchResult = await service.SearchUsersAsync(query, cancellationToken);

        return searchResult.Match<Results<Ok<UserSearchResult>, ProblemHttpResult>>(
            value => TypedResults.Ok(value),
            errors => errors.ToHttpProblemResult());
    }

    private static async Task<Results<Ok<UserIpListResult>, ProblemHttpResult>> GetUserIpListAsync(
        [FromRoute] long userId,
        [FromQuery] int? page,
        [FromQuery] int? pageSize,
        [FromServices] PagingTransformer pagingTransformer,
        [FromServices] IUserIpListProvider provider,
        CancellationToken cancellationToken)
    {
        var (skip, take) = pagingTransformer.GetSkipTake(page, pageSize, DefaultPageSize);
        
        var ipList = await provider.GetUserIpListAsync(
            new UserIpListQuery(userId, skip, take),
            cancellationToken);

        return ipList.Match<Results<Ok<UserIpListResult>, ProblemHttpResult>>(
            value => TypedResults.Ok(value),
            errors => errors.ToHttpProblemResult());
    }

    private static async Task<Results<Ok<UserAccess>, ProblemHttpResult>> GetUserLastAccessAsync(
        [FromRoute] long userId,
        [FromServices] IUserLastAccessProvider lastAccessProvider,
        CancellationToken cancellationToken)
    {
        return (await lastAccessProvider
            .GetLastAccessAsync(userId, cancellationToken))
            .ToHttpResult();
    }
}