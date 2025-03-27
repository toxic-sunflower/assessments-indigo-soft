namespace AccessTracker.Modules;

public static class EndpointRouteBuilderExtensions
{
    public static IEndpointRouteBuilder MapModule<TModule>(this IEndpointRouteBuilder endpoints)
        where TModule : IMinimalApiModule, new()
    {
        var module = new TModule();
        module.RegisterEndpoints(endpoints);
        return endpoints;
    }
}