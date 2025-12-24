using incident_management_system.API.Interfaces;
using incident_management_system.API.Models;

namespace incident_management_system.API.Endpoints;

public static class IncidentEndpoints
{
    public static void MapIncidentEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/incidents").WithTags("Incident Management");

        group.MapGet("/", async (IIncidentService incidentService) =>
        {
            var incidents = await incidentService.GetAllIncidentsAsync();
            return Results.Ok(incidents);
        })
        .WithName("GetAllIncidents")
        .WithSummary("Retrieve all incidents")
        .WithDescription("Gets a list of all incidents in the system.");

        group.MapGet("/{id:int}", async (int id, IIncidentService incidentService) =>
        {
            var incident = await incidentService.GetIncidentByIdAsync(id);
            return incident is not null ? Results.Ok(incident) : Results.NotFound();
        })
        .WithName("GetIncidentById")
        .WithSummary("Retrieve an incident by ID")
        .WithDescription("Gets the details of a specific incident by its ID.");

        group.MapPost("/", async (Incident incident, IIncidentService incidentService) =>
        {
            var createdIncident = await incidentService.CreateIncidentAsync(incident);
            return Results.Created($"/api/incidents/{createdIncident.Id}", createdIncident);
        })
        .WithName("CreateIncident")
        .WithSummary("Create a new incident")
        .WithDescription("Creates a new incident in the system.");

        group.MapPut("/{id:int}", async (int id, Incident updatedIncident, IIncidentService incidentService) =>
        {
            var result = await incidentService.UpdateIncidentAsync(id, updatedIncident);
            return result ? Results.NoContent() : Results.NotFound();
        })
        .WithName("UpdateIncident")
        .WithSummary("Update an existing incident")
        .WithDescription("Updates the details of an existing incident.");

        group.MapDelete("/{id:int}", async (int id, IIncidentService incidentService) =>
        {
            var result = await incidentService.DeleteIncidentAsync(id);
            return result ? Results.NoContent() : Results.NotFound();
        })
        .WithName("DeleteIncident")
        .WithSummary("Delete an incident")
        .WithDescription("Deletes an incident from the system.");
    }
}
