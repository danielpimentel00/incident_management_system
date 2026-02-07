using incident_management_system.API.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace incident_management_system.API.Features.Incidents.GetIncidentById;

public class GetIncidentByIdEndpoint : IEndpoint
{
    public void MapEndpoints(IEndpointRouteBuilder routes)
    {
        routes.MapGet("/api/incidents/{id:int}", async (int id, IMediator mediator) =>
        {
            var incident = await mediator.Send(new GetIncidentByIdQuery(id));
            if (incident is null) return Results.NotFound();
            return Results.Ok(incident);
        })
        .WithTags("Incident Management")
        .WithName("GetIncidentById")
        .WithSummary("Retrieve an incident by ID")
        .WithDescription("Gets the details of a specific incident by its ID.")
        .RequireAuthorization(new AuthorizeAttribute { Roles = "Agent" });
    }
}