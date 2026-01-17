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
        var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

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

        return userDetails;
    }
}
