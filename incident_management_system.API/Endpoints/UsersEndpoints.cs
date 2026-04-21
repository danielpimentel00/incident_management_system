using IMS.Application.Features.Users.Commands.CreateUser;
using IMS.Application.Features.Users.Queries.GetAllUsers;
using IMS.Application.Features.Users.Queries.GetUserById;
using incident_management_system.API.Interfaces;
using MediatR;

namespace incident_management_system.API.Endpoints;

public class UsersEndpoints : IEndpoint
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

        routes.MapGet("/api/users/{id:int}", async (int id, IMediator mediator) =>
        {
            var user = await mediator.Send(new GetUserByIdQuery(id));

            return user is not null ? Results.Ok(user) : Results.NotFound();
        })
        .WithTags("User Management")
        .WithName("GetUserById")
        .WithSummary("Retrieve a user by ID")
        .WithDescription("Gets the details of a specific user by its ID.");

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