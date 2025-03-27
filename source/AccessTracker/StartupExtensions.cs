using AccessTracker.Data;
using AccessTracker.Services;
using Microsoft.EntityFrameworkCore;

namespace AccessTracker;


public static class StartupExtensions
{
    public static IConfigurationBuilder InitConfiguration(
        this IConfigurationBuilder builder,
        IHostEnvironment env)
    {
        builder
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile(
                path: "appsettings.json",
                optional: false,
                reloadOnChange: true)
            .AddJsonFile(
                path: $"appsettings.{env.EnvironmentName}.json",
                optional: false,
                reloadOnChange: true)
            .AddJsonFile(
                path: "appsettings.private.json",
                optional: true,
                reloadOnChange: true);

        return builder;
    }

    public static IServiceCollection AddHostedServices(
        this IServiceCollection services)
    {
        services.AddHostedService<MigrationService>();
        //services.AddHostedService<AggregationService>();
        
        return services;
    }
    
    public static IServiceCollection AddDbContext(
        this IServiceCollection services,
        IConfiguration config)
    {
        var connectionString = config.GetConnectionString("Default") ??
                               throw new InvalidOperationException("No Default connection string found");

        services.AddPooledDbContextFactory<AccessTrackerDbContext>(
            opts => opts.UseNpgsql(connectionString));
        
        return services;
    }
    
    public static IServiceCollection AddSwagger(
        this IServiceCollection services,
        IConfiguration config,
        IHostEnvironment env)
    {
        if (!env.IsDevelopment() && !env.IsStaging())
        {
            return services;
        }
        
        services.AddEndpointsApiExplorer();

        services.AddOpenApiDocument(options =>
        {
            options.Title = "AccessTracker API";
            options.Version = "v1";
        });

        return services;
    }
}