using AccessTracker.Application.EventTracking.Services;
using Microsoft.Extensions.DependencyInjection;

namespace AccessTracker.Application.EventTracking.DependencyInjection;


public static class AccessTrackingRegistrationExtensions
{
    public static IServiceCollection AddEventTracking(
        this IServiceCollection services,
        int trackAttemptsCount,
        TimeSpan trackRetryDelay)
    {
        services.Configure<AccessEventTrackingOptions>(
            options =>
            {
                options.AttemptsCount = trackAttemptsCount;
                options.RetryDelay = trackRetryDelay;
            });
        
        services.AddSingleton<IAccessEventTracker, AccessEventTracker>();
        services.AddSingleton<IAccessEventTrackFailedHandler, AccessEventTrackFailedHandler>();

        return services;
    }
}