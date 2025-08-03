using FluentValidation;

namespace MentorSync.Users.Features.ResetPassword;

public sealed class ResetPasswordCommandValidator : AbstractValidator<ResetPasswordCommand>
{
	public ResetPasswordCommandValidator()
	{
		RuleFor(c => c.Email)
			.NotNull()
			.NotEmpty()
			.EmailAddress();

		RuleFor(c => c.Token)
			.NotNull()
			.NotEmpty();

		RuleFor(c => c.Password)
		   .NotEmpty()
		   .MinimumLength(8)
		   .MaximumLength(100)
		   .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter")
		   .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter")
		   .Matches("[0-9]").WithMessage("Password must contain at least one number")
		   .Matches("[^a-zA-Z0-9]").WithMessage("Password must contain at least one special character");

		RuleFor(c => c.ConfirmPassword)
			.Equal(c => c.Password)
			.WithMessage("Passwords do not match");
	}
}
