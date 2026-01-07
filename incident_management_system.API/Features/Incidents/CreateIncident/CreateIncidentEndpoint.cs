using FluentValidation;
using incident_management_system.API.Interfaces;
using MediatR;

namespace incident_management_system.API.Features.Incidents.CreateIncident;

public class CreateIncidentEndpoint : IEndpoint
{
    public void MapEndpoints(IEndpointRouteBuilder routes)
    {
        routes.MapPost("/api/incidents", async (
            CreateIncidentCommand command,
            IMediator mediator) =>
        {
            var createdIncident = await mediator.Send(command);

            return Results.Created($"/api/incidents/{createdIncident.Id}", createdIncident);
        })
        .WithTags("Incident Management")
        .WithName("CreateIncident")
        .WithSummary("Create a new incident")
        .WithDescription("Creates a new incident in the system.");
    }
}