using IMS.Application.Interfaces.Infrastructure;
using incident_management_system.API.Interfaces;

namespace incident_management_system.API.Endpoints;

public class ResilienceTestEndpoints : IEndpoint
{
    public void MapEndpoints(IEndpointRouteBuilder routes)
    {
        routes.MapPost("/api/resilience-test/success", async (INotificationService notificationService) =>
        {
            await notificationService.SendEscalationNotificationAsync(Random.Shared.Next(1, 10000), "Test success");
            return Results.Ok("Success executed");
        })
        .WithTags("Resilience Testing")
        .WithName("TestSuccess")
        .WithSummary("Test successful notification")
        .WithDescription("Sends a successful notification to test the resilience configuration.");

        routes.MapPost("/api/resilience-test/fail", async (INotificationService notificationService) =>
        {
            await notificationService.SendEscalationNotificationAsync(Random.Shared.Next(1, 10000), "Force 500");
            return Results.Ok("Failure executed");
        })
        .WithTags("Resilience Testing")
        .WithName("TestFailure")
        .WithSummary("Test failure with 500 status")
        .WithDescription("Forces a 500 error to test retry policy and resilience behavior.");

        routes.MapPost("/api/resilience-test/delay", async (INotificationService notificationService) =>
        {
            await notificationService.SendEscalationNotificationAsync(Random.Shared.Next(1, 10000), "Force delay");
            return Results.Ok("Delay executed");
        })
        .WithTags("Resilience Testing")
        .WithName("TestDelay")
        .WithSummary("Test delayed response")
        .WithDescription("Forces a 10-second delay to test timeout and resilience behavior.");
    }
}
