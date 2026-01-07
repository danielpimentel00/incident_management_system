using FluentValidation;

namespace incident_management_system.API.Features.Incidents.UpdateIncident;

public class UpdateIncidentCommandValidator : AbstractValidator<UpdateIncidentCommand>
{
    public UpdateIncidentCommandValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(100).WithMessage("Title cannot exceed 100 characters.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required.")
            .MaximumLength(1000).WithMessage("Description cannot exceed 1000 characters.");

        RuleFor(x => x.Status)
            .IsInEnum().WithMessage("Status must be a valid enum value.");
    }
}
