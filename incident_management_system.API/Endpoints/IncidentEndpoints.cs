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
