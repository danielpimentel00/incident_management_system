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