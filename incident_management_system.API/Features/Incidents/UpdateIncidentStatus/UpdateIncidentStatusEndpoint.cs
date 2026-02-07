using FluentValidation;
using incident_management_system.API.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace incident_management_system.API.Features.Incidents.UpdateIncidentStatus;

public class UpdateIncidentStatusEndpoint : IEndpoint
{
    public void MapEndpoints(IEndpointRouteBuilder routes)
    {
        routes.MapPut("api/incidents/{id:int}/status", async (
            int id,
            UpdateIncidentStatusRequest request,
            IMediator mediator) =>
        {
            var command = new UpdateIncidentStatusCommand
            {
                Id = id,
                Status = request.Status
            };

            var updated = await mediator.Send(command);
            return updated ? Results.NoContent() : Results.NotFound();
        })
        .WithTags("Incident Management")
        .WithName("UpdateIncidentStatus")
        .WithSummary("Update the status of an existing incident")
        .WithDescription("Updates the status of an existing incident.")
        .RequireAuthorization(new AuthorizeAttribute { Roles = "Supervisor" });
    }
}