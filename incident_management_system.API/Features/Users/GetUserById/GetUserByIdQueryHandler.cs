using incident_management_system.API.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace incident_management_system.API.Features.Users.GetUserById;

public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserDetails?>
{
    private readonly IncidentDbContext _dbContext;

    public GetUserByIdQueryHandler(IncidentDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<UserDetails?> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users
            .Select(x => new UserDetails
            {
                Id = x.Id,
                Username = x.Username,
                Email = x.Email,
            }).FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        return user;
    }
}
