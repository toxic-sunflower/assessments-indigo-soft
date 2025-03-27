using AccessTracker.Application.AccessLog;
using AccessTracker.Application.EventTracking.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace AccessTracker.Application;


public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(
        this IServiceCollection services,
        ApplicationSettings settings)
    {
        services.AddEventTracking(
            settings.EventTrackingAttemptsCount,
            settings.EventTrackingRetryInterval);
        
        services.AddAccessLog(
            settings.AccessLogAggregationEntriesBatchSize,
            settings.AccessLogAggregationInterval);

        return services;
    }
}