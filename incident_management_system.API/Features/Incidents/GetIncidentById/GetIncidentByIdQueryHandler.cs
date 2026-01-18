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
            .Include(x => x.CreatedByUser)
            .Include(x => x.Comments)
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        IncidentDetails? incidentDetails = null;
        if (incident is not null)
        {
            incidentDetails = new IncidentDetails
            {
                Id = incident.Id,
                Title = incident.Title,
                Description = incident.Description,
                ResolvedAt = incident.ResolvedAt,
                Status = incident.Status,
                CreatedByUserId = incident.CreatedByUserId,
                CreatedByUserName = incident.CreatedByUser.Username,
                Comments = incident.Comments.Select(c => c.Content).ToList()
            };
        }
        
        return incidentDetails;
    }
}
