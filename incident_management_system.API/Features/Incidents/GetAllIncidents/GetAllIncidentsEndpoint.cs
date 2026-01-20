using incident_management_system.API.Interfaces;
using MediatR;

namespace incident_management_system.API.Features.Incidents.GetAllIncidents;

public class GetAllIncidentsEndpoint : IEndpoint
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
        .WithDescription("Gets a list of all incidents in the system.");
    }
}