using FluentValidation;
using incident_management_system.API.DTOs.Incident;
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
            
            var response = new List<IncidentResponse>();
            foreach (var incident in incidents)
            {
                response.Add(new IncidentResponse
                {
                    Id = incident.Id,
                    Title = incident.Title,
                    Description = incident.Description,
                    ResolvedAt = incident.ResolvedAt,
                    Status = incident.Status
                });
            }

            return Results.Ok(response);
        })
        .WithName("GetAllIncidents")
        .WithSummary("Retrieve all incidents")
        .WithDescription("Gets a list of all incidents in the system.");

        group.MapGet("/{id:int}", async (int id, IIncidentService incidentService) =>
        {
            var incident = await incidentService.GetIncidentByIdAsync(id);

            if (incident is null) return Results.NotFound();

            var response = new IncidentResponse
            {
                Id = incident.Id,
                Title = incident.Title,
                Description = incident.Description,
                ResolvedAt = incident.ResolvedAt,
                Status = incident.Status
            };

            return Results.Ok(response);
        })
        .WithName("GetIncidentById")
        .WithSummary("Retrieve an incident by ID")
        .WithDescription("Gets the details of a specific incident by its ID.");

        group.MapPost("/", async (
            CreateIncidentRequest request, 
            IIncidentService incidentService,
            IValidator<CreateIncidentRequest> requestValidator) =>
        {
            var validationResult = await requestValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                return Results.ValidationProblem(validationResult.ToDictionary());
            }

            var newIncident = new Incident
            {
                Title = request.Title,
                Description = request.Description
            };

            var createdIncident = await incidentService.CreateIncidentAsync(newIncident);

            var response = new IncidentResponse
            {
                Id = createdIncident.Id,
                Title = createdIncident.Title,
                Description = createdIncident.Description,
                ResolvedAt = createdIncident.ResolvedAt,
                Status = createdIncident.Status
            };

            return Results.Created($"/api/incidents/{createdIncident.Id}", response);
        })
        .WithName("CreateIncident")
        .WithSummary("Create a new incident")
        .WithDescription("Creates a new incident in the system.");

        group.MapPut("/{id:int}", async (
            int id, 
            UpdateIncidentRequest request, 
            IIncidentService incidentService,
            IValidator<UpdateIncidentRequest> requestValidator) =>
        {
            var validationResult = await requestValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                return Results.ValidationProblem(validationResult.ToDictionary());
            }

            var updatedIncident = new Incident
            {
                Title = request.Title,
                Description = request.Description,
                Status = request.Status
            };

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

        group.MapPut("/{id:int}/status", async (
            int id, 
            UpdateIncidentStatusRequest request, 
            IIncidentService incidentService,
            IValidator<UpdateIncidentStatusRequest> requestValidator) =>
        {
            var validationResult = await requestValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                return Results.ValidationProblem(validationResult.ToDictionary());
            }

            var result = await incidentService.UpdateIncidentStatusAsync(id, request.Status);
            return result ? Results.NoContent() : Results.NotFound();
        })
        .WithName("UpdateIncidentStatus")
        .WithSummary("Update the status of an existing incident")
        .WithDescription("Updates the status of an existing incident.");
    }
}
