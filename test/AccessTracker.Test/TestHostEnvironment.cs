using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;

namespace AccessTracker.Test;


public class TestHostEnvironment : IHostEnvironment
{
    public static TestHostEnvironment Shared = new ();

    
    private TestHostEnvironment() { }

    public string EnvironmentName { get; set; } = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") ??
                                                  Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ??
                                                  "Development";
    
    public string ApplicationName { get; set; } = "AccessTracker.Test";
    
    public string ContentRootPath { get; set; } = AppContext.BaseDirectory;
    
    public IFileProvider ContentRootFileProvider { get; set; } = new PhysicalFileProvider(AppContext.BaseDirectory);
}
