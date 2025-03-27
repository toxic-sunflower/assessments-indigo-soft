using AccessTracker.Application.AccessLog.Aggregations.UserIpLinks.Services;
using AccessTracker.Domain.AccessLog.Aggregations.UserIpLink;
using Microsoft.Extensions.DependencyInjection;

namespace AccessTracker.Application.AccessLog.Aggregations.UserIpLinks;


public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddUserIpLinksAggregation(
        this IServiceCollection services,
        int accessLogBatchSize,
        TimeSpan aggregationInterval)
    {
        services.AddSingleton<IAccessLogUserIpLinkAggregationMapper, AccessLogUserIpLinkAggregationMapper>();
        
        services.AddAccessLogAggregation<AccessLogUserIpLinkAggregation, AccessLogUserIpLinksAggregationUpdater>(
            accessLogBatchSize,
            aggregationInterval);

        return services;
    }
}