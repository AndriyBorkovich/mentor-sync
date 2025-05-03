using FluentValidation;

namespace MentorSync.Users.Features.ForgotPassword;

public sealed class ForgotPasswordCommandValidator : AbstractValidator<ForgotPasswordCommand>
{
    public ForgotPasswordCommandValidator()
    {
        RuleFor(c => c.Email)
            .NotNull()
            .NotEmpty()
            .EmailAddress();
    }
}
