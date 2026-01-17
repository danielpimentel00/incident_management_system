using incident_management_system.API.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace incident_management_system.API.Features.Users.GetAllUsers;

public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, List<UserListItem>>
{
    private readonly IncidentDbContext _dbContext;

    public GetAllUsersQueryHandler(IncidentDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<UserListItem>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        var users = await _dbContext.Users.ToListAsync(cancellationToken);

        var userListItems = users.Select(user => new UserListItem
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email
        }).ToList();

        return userListItems;
    }
}
