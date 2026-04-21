using IMS.Application.Features.Incidents.Commands.CreateIncident;
using IMS.Application.Features.Incidents.Commands.DeleteIncident;
using IMS.Application.Features.Incidents.Commands.UpdateIncident;
using IMS.Application.Features.Incidents.Commands.UpdateIncidentStatus;
using IMS.Application.Features.Incidents.Queries.GetAllIncidents;
using IMS.Application.Features.Incidents.Queries.GetIncidentById;
using incident_management_system.API.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace incident_management_system.API.Endpoints;

public class IncidentsEndpoints : IEndpoint
{
    public void MapEndpoints(IEndpointRouteBuilder routes)
    {
        routes.MapGet("/api/incidents", async (
            int pageNumber,
            int pageCount,
            IMediator mediator) =>
        {
            var incidents = await mediator.Send(new GetAllIncidentsQuery
            {
                PageNumber = pageNumber,
                PageCount = pageCount
            });
            return Results.Ok(incidents);
        })
        .WithTags("Incident Management")
        .WithName("GetAllIncidents")
        .WithSummary("Retrieve all incidents")
        .WithDescription("Gets a list of all incidents in the system.")
        .RequireAuthorization(new AuthorizeAttribute { Roles = "Agent" });

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
        .WithDescription("Creates a new incident in the system.")
        .RequireAuthorization(new AuthorizeAttribute { Roles = "Supervisor" });

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

        routes.MapDelete("/api/incidents/{id:int}", async (int id, IMediator mediator) =>
        {
            var deleted = await mediator.Send(new DeleteIncidentCommand(id));
            return deleted ? Results.NoContent() : Results.NotFound();
        })
        .WithTags("Incident Management")
        .WithName("DeleteIncident")
        .WithSummary("Delete an incident")
        .WithDescription("Deletes an incident from the system.")
        .RequireAuthorization(new AuthorizeAttribute { Roles = "Admin" });
    }
}
