using AccessTracker.Application.AccessLog.Aggregations;
using Microsoft.Extensions.DependencyInjection;

namespace AccessTracker.Application.AccessLog;


public static class ServiceCollectionExtensions
{
   public static IServiceCollection AddAccessLog(
      this IServiceCollection services,
      int accessLogEventBatchSize,
      TimeSpan accessLogAggregationInterval)
   {
      services.AddAccessLogAggregations(accessLogEventBatchSize, accessLogAggregationInterval);

      services.AddSingleton<AccessLogService>();
      
      services.AddSingleton<IUserIpListProvider>(
         svc => svc.GetRequiredService<AccessLogService>());
      
      services.AddSingleton<IUserLastAccessProvider>(
         svc => svc.GetRequiredService<AccessLogService>());
      
      services.AddSingleton<IUserSearchService>(
         svc => svc.GetRequiredService<AccessLogService>());

      return services;
   }
}