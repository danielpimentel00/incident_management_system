using incident_management_system.API.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace incident_management_system.API.Features.Incidents.GetIncidentById;

public class GetIncidentByIdQueryHandler : IRequestHandler<GetIncidentByIdQuery, IncidentDetails?>
{
    private readonly IncidentDbContext _dbContext;

    public GetIncidentByIdQueryHandler(IncidentDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IncidentDetails?> Handle(GetIncidentByIdQuery request, CancellationToken cancellationToken)
    {
        var incident = await _dbContext.Incidents
            .AsNoTracking()
            .Select(x => new IncidentDetails
            {
                Id = x.Id,
                Title = x.Title,
                Description = x.Description,
                ResolvedAt = x.ResolvedAt,
                Status = x.Status,
                CreatedByUserId = x.CreatedByUserId,
                CreatedByUserName = x.CreatedByUser.Username,
                Comments = x.Comments.Select(c => c.Content).ToList()
            })
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        
        return incident;
    }
}
