using incident_management_system.API.Infrastructure;
using incident_management_system.API.Models;
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
        var users = await _dbContext.Users
            .Select(x => new UserListItem
            {
                Id = x.Id,
                Username = x.Username,
                Email = x.Email
            }).ToListAsync(cancellationToken);

        return users;
    }
}
