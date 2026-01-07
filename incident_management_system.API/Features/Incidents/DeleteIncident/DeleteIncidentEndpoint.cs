using incident_management_system.API.Interfaces;
using MediatR;

namespace incident_management_system.API.Features.Incidents.DeleteIncident;

public class DeleteIncidentEndpoint : IEndpoint
{
    public void MapEndpoints(IEndpointRouteBuilder routes)
    {
        routes.MapDelete("/api/incidents/{id:int}", async (int id, IMediator mediator) =>
        {
            var deleted = await mediator.Send(new DeleteIncidentCommand(id));
            return deleted ? Results.NoContent() : Results.NotFound();
        })
        .WithTags("Incident Management")
        .WithName("DeleteIncident")
        .WithSummary("Delete an incident")
        .WithDescription("Deletes an incident from the system.");
    }
}