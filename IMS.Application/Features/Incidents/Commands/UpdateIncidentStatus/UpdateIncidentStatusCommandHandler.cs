using IMS.Application.Events;
using IMS.Application.Interfaces.Infrastructure;
using IMS.Application.Interfaces.Persistance;
using IMS.Application.Shared;
using IMS.Domain.Enums;
using MediatR;

namespace IMS.Application.Features.Incidents.Commands.UpdateIncidentStatus;

public class UpdateIncidentStatusCommandHandler : IRequestHandler<UpdateIncidentStatusCommand, bool>
{
    private readonly IIncidentsRepository _incidentsRepository;
    private readonly ICacheService _cache;
    private readonly IEventBus _eventBus;

    public UpdateIncidentStatusCommandHandler(
        IIncidentsRepository incidentsRepository,
        ICacheService cache,
        IEventBus eventBus)
    {
        _incidentsRepository = incidentsRepository;
        _cache = cache;
        _eventBus = eventBus;
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
            await _eventBus.PublishAsync(new IncidentEscalatedEvent(
                request.Id,
                request.Status.ToString(),
                DateTime.UtcNow));
        }

        return true;
    }
}
