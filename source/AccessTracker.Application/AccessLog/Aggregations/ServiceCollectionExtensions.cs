using AccessTracker.Application.AccessLog.Aggregations.Common.Services;
using AccessTracker.Application.AccessLog.Aggregations.Common.Services.Aggregator;
using AccessTracker.Application.AccessLog.Aggregations.Common.Services.Updater;
using AccessTracker.Application.AccessLog.Aggregations.LastAccess;
using AccessTracker.Application.AccessLog.Aggregations.UserIpLinks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace AccessTracker.Application.AccessLog.Aggregations;


public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAccessLogAggregation<TAggregation, TAggregationUpdater>(
        this IServiceCollection services,
        int logEntriesBatchSize,
        TimeSpan aggregationInterval)
        where TAggregationUpdater : class, IAccessLogAggregationUpdater<TAggregation>
    {
        services.Configure<AccessLogAggregatorOptions<TAggregation>>(
            x =>
            {
                x.LogEntriesBatchSize = logEntriesBatchSize;
                x.AggregationInterval = aggregationInterval;
            });

        services.AddSingleton<IAccessLogAggregatorEntriesLoader<TAggregation>,
                              AccessLogAggregatorEntriesLoader<TAggregation>>();

        services.AddSingleton<AccessLogAggregator<TAggregation>>();
        services.AddSingleton<IAccessLogAggregationUpdater<TAggregation>, TAggregationUpdater>();
        
        services.AddSingleton<IAccessLogAggregator<TAggregation>>(
            svc => svc.GetRequiredService<AccessLogAggregator<TAggregation>>());
        
        services.TryAddEnumerable(
            ServiceDescriptor.Describe(
                typeof(IAccessLogAggregator),
                typeof(AccessLogAggregator<TAggregation>),
                ServiceLifetime.Singleton));
        
        return services;
    }
    
    public static IServiceCollection AddAccessLogAggregations(
        this IServiceCollection services,
        int aggregationEventBatchSize,
        TimeSpan aggregationInterval)
    {
        services.AddSingleton<IAccessLogAggregationTypeMapper, AccessLogAggregationTypeMapper>();
        services.AddSingleton<IAccessLogAggregationCheckpointUpdater, AccessLogAggregationCheckpointUpdater>();
        services.AddLastAccessAggregation(aggregationEventBatchSize, aggregationInterval);
        services.AddUserIpLinksAggregation(aggregationEventBatchSize, aggregationInterval);
        
        return services;
    }
}