using incident_management_system.API.Enums;
using incident_management_system.API.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace incident_management_system.API.Features.Incidents.UpdateIncidentStatus;

public class UpdateIncidentStatusCommandHandler : IRequestHandler<UpdateIncidentStatusCommand, bool>
{
    private readonly IncidentDbContext _dbContext;

    public UpdateIncidentStatusCommandHandler(IncidentDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> Handle(UpdateIncidentStatusCommand request, CancellationToken cancellationToken)
    {
        var incident = await _dbContext.Incidents.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        if (incident is null)
        {
            return false;
        }

        incident.Status = request.Status;
        if (incident.Status == IncidentStatus.Resolved) incident.ResolvedAt = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync();

        return true;
    }
}
