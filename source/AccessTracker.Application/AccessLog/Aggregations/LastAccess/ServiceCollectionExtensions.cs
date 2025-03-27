using AccessTracker.Application.AccessLog.Aggregations.LastAccess.Services;
using AccessTracker.Domain.AccessLog.Aggregations.LastAccess;
using Microsoft.Extensions.DependencyInjection;

namespace AccessTracker.Application.AccessLog.Aggregations.LastAccess;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddLastAccessAggregation(
        this IServiceCollection services,
        int accessLogBatchSize,
        TimeSpan aggregationInterval)
    {
        services.AddSingleton<IAccessLogLastAccessAggregationMapper, AccessLogLastAccessAggregationMapper>();
        
        services.AddAccessLogAggregation<AccessLogLastAccessAggregation, AccessLogLastAccessAggregationUpdater>(
            accessLogBatchSize,
            aggregationInterval);

        return services;
    }
}