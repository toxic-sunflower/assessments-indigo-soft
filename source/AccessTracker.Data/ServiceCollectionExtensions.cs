using AccessTracker.Data.Repositories;
using AccessTracker.Data.Storages;
using AccessTracker.Domain.AccessLog;
using AccessTracker.Domain.AccessLog.Aggregations.Common.Checkpoints;
using AccessTracker.Domain.AccessLog.Aggregations.LastAccess;
using AccessTracker.Domain.AccessLog.Aggregations.UserIpLink;
using Microsoft.Extensions.DependencyInjection;

namespace AccessTracker.Data;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddSingleton<IStorage, Storage>();
        services.AddSingleton<IAccessLogRepository, AccessLogRepository>();
        services.AddSingleton<IAccessLogAggregationCheckpointRepository, AccessLogAggregationCheckpointRepository>();
        services.AddSingleton<IAccessLogLastAccessAccessLogAggregationRepository, AccessLogLastAccessAccessLogAggregationRepository>();
        services.AddSingleton<IAccessLogUserIpLinkAccessLogAggregationRepository, AccessLogUserIpLinkAccessLogAggregationRepository>();

        return services;
    }
}