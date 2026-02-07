using FluentValidation;
using incident_management_system.API.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace incident_management_system.API.Features.Incidents.UpdateIncident;

public class UpdateIncidentEndpoint : IEndpoint
{
    public void MapEndpoints(IEndpointRouteBuilder routes)
    {
        routes.MapPut("/api/incidents/{id:int}", async (
            int id,
            UpdateIncidentRequest request,
            IMediator mediator) =>
        {
            var command = new UpdateIncidentCommand
            {
                Id = id,
                Title = request.Title,
                Description = request.Description,
                Status = request.Status
            };

            var updated = await mediator.Send(command);
            return updated ? Results.NoContent() : Results.NotFound();
        })
        .WithTags("Incident Management")
        .WithName("UpdateIncident")
        .WithSummary("Update an existing incident")
        .WithDescription("Updates the details of an existing incident.")
        .RequireAuthorization(new AuthorizeAttribute { Roles = "Supervisor" });
    }
}