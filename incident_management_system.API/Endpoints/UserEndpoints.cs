using incident_management_system.API.Interfaces;
using incident_management_system.API.Models;

namespace incident_management_system.API.Endpoints;

public static class UserEndpoints
{
    public static void MapUserEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/users").WithTags("User Management");

        group.MapGet("/", async (IUserService userService) =>
        {
            var users = await userService.GetAllUsersAsync();
            return Results.Ok(users);
        })
        .WithName("GetAllUsers")
        .WithSummary("Retrieve all users")
        .WithDescription("Gets a list of all users in the system.");

        group.MapGet("/{id:int}", async (int id, IUserService userService) =>
        {
            var user = await userService.GetUserByIdAsync(id);
            return user is not null ? Results.Ok(user) : Results.NotFound();
        })
        .WithName("GetUserById")
        .WithSummary("Retrieve a user by ID")
        .WithDescription("Gets the details of a specific user by its ID.");

        group.MapPost("/", async (User user, IUserService userService) =>
        {
            var createdUser = await userService.CreateUserAsync(user);
            return Results.Created($"/api/users/{createdUser.Id}", createdUser);
        })
        .WithName("CreateUser")
        .WithSummary("Create a new user")
        .WithDescription("Creates a new user in the system.");
    }
}