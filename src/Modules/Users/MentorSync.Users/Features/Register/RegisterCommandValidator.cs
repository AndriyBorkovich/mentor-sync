using FluentValidation;
using MentorSync.SharedKernel;

namespace MentorSync.Users.Features.Register;

public sealed class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .MaximumLength(256);

        RuleFor(x => x.UserName)
            .NotEmpty()
            .MinimumLength(4)
            .MaximumLength(50);
        
        RuleFor(x => x.Role)
            .NotEmpty()
            .Must(x => x.Equals(Roles.Admin, StringComparison.InvariantCultureIgnoreCase)
                        || x.Equals(Roles.Mentor, StringComparison.InvariantCultureIgnoreCase)
                        || x.Equals(Roles.Mentee, StringComparison.InvariantCultureIgnoreCase))
            .WithMessage("Such role doesn't exist");

        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(8)
            .MaximumLength(100)
            .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter")
            .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter")
            .Matches("[0-9]").WithMessage("Password must contain at least one number")
            .Matches("[^a-zA-Z0-9]").WithMessage("Password must contain at least one special character");

        RuleFor(x => x.ConfirmPassword)
            .Equal(x => x.Password)
            .WithMessage("Passwords do not match");
    }
}