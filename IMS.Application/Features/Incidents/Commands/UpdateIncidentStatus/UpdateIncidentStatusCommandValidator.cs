using FluentValidation;
using IMS.Application.Interfaces.Persistance;

namespace IMS.Application.Features.Incidents.Commands.UpdateIncidentStatus;

public class UpdateIncidentStatusCommandValidator : AbstractValidator<UpdateIncidentStatusCommand>
{
    private readonly IIncidentsRepository _incidentsRepository;

    public UpdateIncidentStatusCommandValidator(IIncidentsRepository incidentsRepository)
    {
        _incidentsRepository = incidentsRepository;

        RuleFor(x => x.Status)
            .IsInEnum().WithMessage("Status must be a valid enum value.");

        RuleFor(x => x.Id)
            .MustAsync(IncidentExistsAsync).WithMessage("Incident with the given Id does not exist.");
    }

    private async Task<bool> IncidentExistsAsync(int id, CancellationToken cancellationToken)
    {
        var incident = await _incidentsRepository.GetIncidentByIdAsync(id);
        return incident is not null;
    }
}
