using incident_management_system.API.Infrastructure;
using incident_management_system.API.Models;
using MediatR;

namespace incident_management_system.API.Features.Users.CreateUser;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, CreatedUser>
{
    private readonly IncidentDbContext _dbContext;

    public CreateUserCommandHandler(IncidentDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<CreatedUser> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var newUser = new User
        {
            Email = request.Email,
            Username = request.Username,
        };

        var createdUser = await _dbContext.Users.AddAsync(newUser, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        CreatedUser response = new()
        {
            Id = createdUser.Entity.Id,
            Email = createdUser.Entity.Email,
            Username = createdUser.Entity.Username
        };

        return response;
    }
}
