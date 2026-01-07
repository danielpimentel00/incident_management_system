using incident_management_system.API.Interfaces;
using MediatR;

namespace incident_management_system.API.Features.Users.GetUserById;

public class GetUserByIdEndpoint : IEndpoint
{
    public void MapEndpoints(IEndpointRouteBuilder routes)
    {
        routes.MapGet("/api/users/{id:int}", async (int id, IMediator mediator) =>
        {
            var user = await mediator.Send(new GetUserByIdQuery(id));

            return user is not null ? Results.Ok(user) : Results.NotFound();
        })
        .WithTags("User Management")
        .WithName("GetUserById")
        .WithSummary("Retrieve a user by ID")
        .WithDescription("Gets the details of a specific user by its ID.");
    }
}