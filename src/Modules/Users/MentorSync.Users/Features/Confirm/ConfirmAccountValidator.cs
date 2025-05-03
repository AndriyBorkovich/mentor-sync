using FluentValidation;

namespace MentorSync.Users.Features.Confirm;

public sealed class ConfirmAccountValidator : AbstractValidator<ConfirmAccountCommand>
{
    public ConfirmAccountValidator()
    {
        RuleFor(c => c.Email).NotNull().NotEmpty().EmailAddress();
        RuleFor(c => c.Token).NotNull().NotEmpty();
    }
}
