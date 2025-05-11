using FluentValidation;
using MentorSync.SharedKernel;

namespace MentorSync.Users.Features.Confirm;

public sealed class ConfirmAccountValidator : AbstractValidator<ConfirmAccountCommand>
{
    public ConfirmAccountValidator()
    {
        RuleFor(c => c.Email).NotNull().NotEmpty().EmailAddress().MaximumLength(GeneralConstants.MaxEmailLength);
        RuleFor(c => c.Token).NotNull().NotEmpty();
    }
}
