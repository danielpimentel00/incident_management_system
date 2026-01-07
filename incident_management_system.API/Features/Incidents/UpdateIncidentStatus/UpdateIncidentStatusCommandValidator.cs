using FluentValidation;

namespace incident_management_system.API.Features.Incidents.UpdateIncidentStatus;

public class UpdateIncidentStatusCommandValidator : AbstractValidator<UpdateIncidentStatusCommand>
{
    public UpdateIncidentStatusCommandValidator()
    {
        RuleFor(x => x.Status)
            .IsInEnum().WithMessage("Status must be a valid enum value.");
    }
}
