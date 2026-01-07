using incident_management_system.API.Infrastructure;
using MediatR;

namespace incident_management_system.API.Features.Users.GetAllUsers;

public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, List<UserListItem>>
{
    private readonly UserInMemoryDb _userInMemoryDb;

    public GetAllUsersQueryHandler(UserInMemoryDb userInMemoryDb)
    {
        _userInMemoryDb = userInMemoryDb;
    }

    public Task<List<UserListItem>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        var users = _userInMemoryDb.Users.ToList();

        var userListItems = users.Select(user => new UserListItem
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email
        }).ToList();

        return Task.FromResult(userListItems);
    }
}
