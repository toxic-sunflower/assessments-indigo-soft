using AccessTracker.Application;
using AccessTracker.Data;
using AccessTracker.Helpers;
using AccessTracker.Modules.AccessLog;
using AccessTracker.Modules.EventTracking;
using AccessTracker.Modules;

namespace AccessTracker;


public static class Program
{
    public static void Main(string[] args) => 
        CreateHostBuilder(args)
            .Build()
            .Run();
    
    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.ConfigureServices(ConfigureServices);
                webBuilder.Configure(ConfigureApp);
            });

    private static void ConfigureApp(WebHostBuilderContext context, IApplicationBuilder app)
    {
        if (context.HostingEnvironment.IsDevelopment() || context.HostingEnvironment.IsStaging())
        {
            app.UseOpenApi();
            app.UseSwaggerUi();
        }

        app.UseRouting();
        
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapModule<AccessLogApiModule>();
            endpoints.MapModule<TrackEventApiModule>();
        });
    }

    private static void ConfigureServices(WebHostBuilderContext context, IServiceCollection services)
    {
        var applicationSettings = new ApplicationSettings();
        
        context.Configuration
            .GetSection("ApplicationServices")
            .Bind(applicationSettings);

        services
            .AddDbContext(context.Configuration)
            .AddSwagger(context.Configuration, context.HostingEnvironment)
            .AddHostedServices()
            .AddApplicationServices(applicationSettings)
            .AddRepositories()
            .AddSingleton(TimeProvider.System)
            .AddSingleton(PagingTransformer.Shared);
    }
}