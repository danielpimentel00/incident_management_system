namespace incident_management_system.API.Health;

public static class HealthEndpoints
{
    public static void MapHealthEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/health").WithTags("Health");
        group.MapGet("/", () =>
        {
            return Results.Ok(new
            {
                status = "Healthy",
                timestamp = DateTime.UtcNow,
                service = "Incident Management API"
            });
        }).WithName("GetHealthStatus")
          .WithSummary("Retrieve health status")
          .WithDescription("Checks the health status of the API.");
    }
}
