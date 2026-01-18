using FluentValidation;
using incident_management_system.API.Infrastructure;

namespace incident_management_system.API.Features.Incidents.CreateIncident;

public class CreateIncidentCommandValidator : AbstractValidator<CreateIncidentCommand>
{
    private readonly IncidentDbContext _dbContext;

    public CreateIncidentCommandValidator(IncidentDbContext dbContext)
    {
        _dbContext = dbContext;

        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(100).WithMessage("Title cannot exceed 100 characters.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required.")
            .MaximumLength(1000).WithMessage("Description cannot exceed 1000 characters.");

        RuleFor(x => x.CreatedByUserId)
            .Must(ValidateUserExists)
            .WithMessage("The specified user does not exist.");
    }

    public bool ValidateUserExists(int userId)
    {
        var user = _dbContext.Users.Find(userId);

        return user != null;
    }
}