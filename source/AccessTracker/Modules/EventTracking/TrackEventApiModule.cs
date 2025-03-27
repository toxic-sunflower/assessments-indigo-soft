using AccessTracker.Application.EventTracking;
using AccessTracker.Helpers;
using AccessTracker.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace AccessTracker.Modules.EventTracking;

public class TrackEventApiModule : IMinimalApiModule
{
    public void RegisterEndpoints(IEndpointRouteBuilder routes)
    {
        routes.MapPost("/api/events", TrackEventAsync).WithTags("events");
    }


    private static async Task<Results<Ok, ProblemHttpResult>> TrackEventAsync(
        [FromBody] AccessEvent evt,
        [FromServices] IAccessEventTracker eventTracker,
        CancellationToken cancellationToken)
    {
        return (await eventTracker
            .TrackAccessEventAsync(evt, cancellationToken))
            .ToHttpResult();
    }
}