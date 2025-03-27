using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using AccessTracker.Helpers;

namespace AccessTracker.Test;


public abstract class TestBase<T> : IAsyncLifetime where T : notnull
{
    private IServiceProvider? _rootServices;
    private IServiceScope? _scope;
    
    protected IConfiguration Configuration { get; private set; }
    protected IHostEnvironment Environment => TestHostEnvironment.Shared;
    protected IServiceProvider Services => _scope!.ServiceProvider;
    protected T Target { get; private set; } 
    protected virtual ServiceLifetime TargetLifeTime => ServiceLifetime.Singleton;
    
    
    public async Task InitializeAsync()
    {
        Configuration = new ConfigurationBuilder()
            .InitConfiguration(Environment)
            .Build();
        
        var services = new ServiceCollection();
        
        ConfigureServices(services);

        services.AddLogging(x => x.AddConsole());
        services.Add(new ServiceDescriptor(typeof(T), typeof(T), TargetLifeTime));

        _rootServices = services.BuildServiceProvider();
        _scope = _rootServices.CreateScope();
        
        await SetUpAsync();

        Target = Services.GetRequiredService<T>();
    }

    public async Task DisposeAsync()
    {
        await TearDownAsync();
        _scope?.Dispose();
    }

    protected virtual void ConfigureServices(IServiceCollection services) { }

    protected virtual Task SetUpAsync() => Task.CompletedTask;

    protected virtual Task TearDownAsync() => Task.CompletedTask;
}