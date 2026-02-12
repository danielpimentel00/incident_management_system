using IMS.Domain.Enums;
using incident_management_system.API.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace incident_management_system.API.Features.Incidents.UpdateIncident;

public class UpdateIncidentCommandHandler : IRequestHandler<UpdateIncidentCommand, bool>
{
    private readonly IncidentDbContext _dbContext;

    public UpdateIncidentCommandHandler(IncidentDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> Handle(UpdateIncidentCommand request, CancellationToken cancellationToken)
    {
        var incident = await _dbContext.Incidents.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        if (incident is null)
        {
            return false;
        }

        incident.Title = request.Title;
        incident.Description = request.Description;
        incident.Status = request.Status;

        if (incident.Status == IncidentStatus.Resolved) incident.ResolvedAt = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync();

        return true;
    }
}
