using IMS.Application.Interfaces.Persistance;
using IMS.Domain.Entities;
using MediatR;

namespace IMS.Application.Features.Users.Commands.CreateUser;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, CreatedUser>
{
    private readonly IUsersRepository _usersRepository;

    public CreateUserCommandHandler(IUsersRepository usersRepository)
    {
        _usersRepository = usersRepository;
    }

    public async Task<CreatedUser> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var newUser = new User
        {
            Email = request.Email,
            Username = request.Username,
        };

        var createdUser = await _usersRepository.CreateUserAsync(newUser);

        CreatedUser response = new()
        {
            Id = createdUser.Id,
            Email = createdUser.Email,
            Username = createdUser.Username
        };

        return response;
    }
}