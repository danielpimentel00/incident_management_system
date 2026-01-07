using incident_management_system.API.Infrastructure;
using MediatR;

namespace incident_management_system.API.Features.Users.GetUserById;

public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserDetails?>
{
    private readonly UserInMemoryDb _userInMemoryDb;

    public GetUserByIdQueryHandler(UserInMemoryDb userInMemoryDb)
    {
        _userInMemoryDb = userInMemoryDb;
    }

    public Task<UserDetails?> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = _userInMemoryDb.Users.FirstOrDefault(u => u.Id == request.Id);

        UserDetails? userDetails = null;
        if (user is not null)
        {
            userDetails = new()
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
            };
        }

        return Task.FromResult(userDetails);
    }
}
