using FluentValidation;

namespace MentorSync.Users.Features.Bio.Add;

public sealed class AddBioCommandValidator : AbstractValidator<AddBioRequest>
{
    public AddBioCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty().NotNull().GreaterThan(0);
        RuleFor(x => x.Bio)
            .NotEmpty()
            .MinimumLength(5)
            .MaximumLength(500);
    }
}
