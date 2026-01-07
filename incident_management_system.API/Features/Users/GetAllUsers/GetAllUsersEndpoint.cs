using incident_management_system.API.Interfaces;
using MediatR;

namespace incident_management_system.API.Features.Users.GetAllUsers;

public class GetAllUsersEndpoint : IEndpoint
{
    public void MapEndpoints(IEndpointRouteBuilder routes)
    {
        routes.MapGet("/api/users/", async (IMediator mediator) =>
        {
            var users = await mediator.Send(new GetAllUsersQuery());
            return Results.Ok(users);
        })
        .WithTags("User Management")
        .WithName("GetAllUsers")
        .WithSummary("Retrieve all users")
        .WithDescription("Gets a list of all users in the system.");
    }
}