using MediatR;

namespace incident_management_system.API.Features.Users.CreateUser;

public class CreateUserCommand : IRequest<CreatedUser>
{
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}
