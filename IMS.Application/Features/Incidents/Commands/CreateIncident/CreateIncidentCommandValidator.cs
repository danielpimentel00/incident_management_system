using FluentValidation;
using IMS.Application.Interfaces.Persistance;

namespace IMS.Application.Features.Incidents.Commands.CreateIncident;

public class CreateIncidentCommandValidator : AbstractValidator<CreateIncidentCommand>
{
    private readonly IUsersRepository _usersRepository;

    public CreateIncidentCommandValidator(IUsersRepository usersRepository)
    {
        _usersRepository = usersRepository;

        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(100).WithMessage("Title cannot exceed 100 characters.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required.")
            .MaximumLength(1000).WithMessage("Description cannot exceed 1000 characters.");

        RuleFor(x => x.CreatedByUserId)
            .MustAsync(ValidateUserExistsAsync)
            .WithMessage("The specified user does not exist.");
    }

    public async Task<bool> ValidateUserExistsAsync(int userId, CancellationToken cancellationToken)
    {
        var user = _usersRepository.GetUserByIdAsync(userId);

        return user != null;
    }
}
