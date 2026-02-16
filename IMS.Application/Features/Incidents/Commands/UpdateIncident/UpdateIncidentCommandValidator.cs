using FluentValidation;
using IMS.Application.Interfaces.Persistance;

namespace IMS.Application.Features.Incidents.Commands.UpdateIncident;

public class UpdateIncidentCommandValidator : AbstractValidator<UpdateIncidentCommand>
{
    private readonly IIncidentsRepository _incidentsRepository;

    public UpdateIncidentCommandValidator(IIncidentsRepository incidentsRepository)
    {
        _incidentsRepository = incidentsRepository;

        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(100).WithMessage("Title cannot exceed 100 characters.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required.")
            .MaximumLength(1000).WithMessage("Description cannot exceed 1000 characters.");

        RuleFor(x => x.Status)
            .IsInEnum().WithMessage("Status must be a valid enum value.");

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
