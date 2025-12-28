using FluentValidation;
using incident_management_system.API.DTOs.User;
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

            var response = new List<UserResponse>();

            foreach (var user in users)
            {
                response.Add(new UserResponse
                {
                    Id = user.Id,
                    Username = user.Username,
                    Email = user.Email
                });
            }

            return Results.Ok(response);
        })
        .WithName("GetAllUsers")
        .WithSummary("Retrieve all users")
        .WithDescription("Gets a list of all users in the system.");

        group.MapGet("/{id:int}", async (int id, IUserService userService) =>
        {
            var user = await userService.GetUserByIdAsync(id);

            if (user is null) return Results.NotFound();

            var response = new UserResponse
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email
            };

            return Results.Ok(response);
        })
        .WithName("GetUserById")
        .WithSummary("Retrieve a user by ID")
        .WithDescription("Gets the details of a specific user by its ID.");

        group.MapPost("/", async (
            CreateUserRequest request, 
            IUserService userService,
            IValidator<CreateUserRequest> requestValidator) =>
        {
            var validationResult = await requestValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                return Results.ValidationProblem(validationResult.ToDictionary());
            }

            var newUser = new User
            {
                Username = request.Username,
                Email = request.Email
            };

            var createdUser = await userService.CreateUserAsync(newUser);

            var response = new UserResponse
            {
                Id = createdUser.Id,
                Username = createdUser.Username,
                Email = createdUser.Email
            };

            return Results.Created($"/api/users/{response.Id}", response);
        })
        .WithName("CreateUser")
        .WithSummary("Create a new user")
        .WithDescription("Creates a new user in the system.");
    }
}