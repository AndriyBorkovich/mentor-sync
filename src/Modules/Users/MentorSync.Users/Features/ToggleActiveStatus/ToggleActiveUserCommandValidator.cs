using FluentValidation;

namespace MentorSync.Users.Features.ToggleActiveStatus;

public sealed class ToggleActiveUserCommandValidator : AbstractValidator<ToggleActiveUserCommand>
{
    public ToggleActiveUserCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotNull()
            .GreaterThan(0);
    }
}
