using FluentValidation;
using incident_management_system.API.DTOs.Incident;

namespace incident_management_system.API.Validators.Incident;

public class UpdateIncidentStatusRequestValidator : AbstractValidator<UpdateIncidentStatusRequest>
{
    public UpdateIncidentStatusRequestValidator()
    {
        RuleFor(x => x.Status)
            .IsInEnum().WithMessage("Status must be a valid enum value.");
    }
}