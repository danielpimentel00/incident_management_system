using FluentValidation;
using incident_management_system.API.Interfaces;
using MediatR;

namespace incident_management_system.API.Features.Users.CreateUser;

public class CreateUserEndpoint : IEndpoint
{
    public void MapEndpoints(IEndpointRouteBuilder routes)
    {
        routes.MapPost("/api/users/", async (CreateUserCommand command, IMediator mediator) =>
        {
            var createdUser = await mediator.Send(command);

            return Results.Created($"/api/users/{createdUser.Id}", createdUser);
        })
        .WithTags("User Management")
        .WithName("CreateUser")
        .WithSummary("Create a new user")
        .WithDescription("Creates a new user in the system.");
    }
}