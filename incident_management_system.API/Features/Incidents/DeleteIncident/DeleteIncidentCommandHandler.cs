using incident_management_system.API.Infrastructure;
using MediatR;

namespace incident_management_system.API.Features.Incidents.DeleteIncident;

public class DeleteIncidentCommandHandler : IRequestHandler<DeleteIncidentCommand, bool>
{
    private readonly IncidentDbContext _dbContext;

    public DeleteIncidentCommandHandler(IncidentDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> Handle(DeleteIncidentCommand request, CancellationToken cancellationToken)
    {
        var incident = await _dbContext.Incidents.FindAsync(request.Id, cancellationToken);
        if (incident is null)
        {
            return false;
        }

        _dbContext.Incidents.Remove(incident);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return true;
    }
}
