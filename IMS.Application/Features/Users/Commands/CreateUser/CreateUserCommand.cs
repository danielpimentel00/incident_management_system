using MediatR;

namespace IMS.Application.Features.Users.Commands.CreateUser;

public class CreateUserCommand : IRequest<CreatedUser>
{
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}
