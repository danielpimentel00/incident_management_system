using FluentValidation;
using IMS.Application.Interfaces.Persistance;

namespace IMS.Application.Features.Incidents.Commands.DeleteIncident;

public class DeleteIncidentCommandValidator : AbstractValidator<DeleteIncidentCommand>
{
    private readonly IIncidentsRepository _incidentsRepository;

    public DeleteIncidentCommandValidator(IIncidentsRepository incidentsRepository)
    {
        _incidentsRepository = incidentsRepository;

        RuleFor(x => x.Id)
            .MustAsync(ValidateIncidentExistsAsync)
            .WithMessage("The specified incident does not exist.");
    }

    private async Task<bool> ValidateIncidentExistsAsync(int id, CancellationToken cancellationToken)
    {
        var incident = await _incidentsRepository.GetIncidentByIdAsync(id);
        return incident != null;
    }
}
