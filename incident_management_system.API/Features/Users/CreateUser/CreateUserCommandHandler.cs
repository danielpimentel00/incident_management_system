using incident_management_system.API.Infrastructure;
using incident_management_system.API.Models;
using MediatR;

namespace incident_management_system.API.Features.Users.CreateUser;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, CreatedUser>
{
    private readonly UserInMemoryDb _userInMemoryDb;

    public CreateUserCommandHandler(UserInMemoryDb userInMemoryDb)
    {
        _userInMemoryDb = userInMemoryDb;
    }

    public Task<CreatedUser> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var newId = _userInMemoryDb.Users.Count > 0
            ? _userInMemoryDb.Users.Max(u => u.Id) + 1
            : 1;
        var newUser = new User
        {
            Id = newId,
            Email = request.Email,
            Username = request.Username,
        };

        _userInMemoryDb.Users.Add(newUser);

        CreatedUser createdUser = new()
        {
            Id = newUser.Id,
            Email = newUser.Email,
            Username = newUser.Username
        };

        return Task.FromResult(createdUser);
    }
}
