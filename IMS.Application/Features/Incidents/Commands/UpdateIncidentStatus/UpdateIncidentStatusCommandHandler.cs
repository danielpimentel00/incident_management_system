using IMS.Application.Interfaces.Infrastructure;
using IMS.Application.Interfaces.Persistance;
using IMS.Application.Shared;
using IMS.Domain.Enums;
using MediatR;

namespace IMS.Application.Features.Incidents.Commands.UpdateIncidentStatus;

public class UpdateIncidentStatusCommandHandler : IRequestHandler<UpdateIncidentStatusCommand, bool>
{
    private readonly IIncidentsRepository _incidentsRepository;
    private readonly INotificationService _notificationService;
    private readonly ICacheService _cache;

    public UpdateIncidentStatusCommandHandler(
        IIncidentsRepository incidentsRepository,
        INotificationService notificationService,
        ICacheService cache)
    {
        _incidentsRepository = incidentsRepository;
        _notificationService = notificationService;
        _cache = cache;
    }

    public async Task<bool> Handle(UpdateIncidentStatusCommand request, CancellationToken cancellationToken)
    {
        var incident = await _incidentsRepository.GetIncidentByIdAsync(request.Id);

        if (incident == null)
            throw new KeyNotFoundException($"Incident with ID {request.Id} not found.");

        incident.UpdateStatus(request.Status);

        await _incidentsRepository.UpdateIncidentAsync(incident);

        await _cache.RemoveAsync(CacheKeys.IncidentById(request.Id));
        await _cache.RemoveByPrefixAsync(CacheKeys.IncidentsListPrefix);
        await _cache.RemoveAsync(CacheKeys.OpenIncidents);

        if (request.Status == IncidentStatus.Resolved)
        {
            await _notificationService.SendEscalationNotificationAsync(
                request.Id,
                $"Incident #{request.Id} has been resolved.");
        }

        return true;
    }
}
