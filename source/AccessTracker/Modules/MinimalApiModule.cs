namespace AccessTracker.Modules;

public interface IMinimalApiModule
{
    void RegisterEndpoints(IEndpointRouteBuilder routes);
}