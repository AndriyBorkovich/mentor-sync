using FluentValidation;

namespace MentorSync.Users.Features.Login;

/// <summary>
/// Validator for <see cref="LoginCommand"/>
/// </summary>
public sealed class LoginCommandValidator : AbstractValidator<LoginCommand>
{
	/// <summary>
	/// Initializes a new instance of the <see cref="LoginCommandValidator"/> class.
	/// </summary>
	public LoginCommandValidator()
	{
		RuleFor(x => x.Email)
			.NotNull()
			.NotEmpty()
			.EmailAddress()
			.MaximumLength(GeneralConstants.MaxEmailLength);

		RuleFor(x => x.Password)
			.NotNull()
			.NotEmpty()
			.MinimumLength(GeneralConstants.MinPasswordLength)
			.Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter")
			.Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter")
			.Matches("[0-9]").WithMessage("Password must contain at least one number")
			.Matches("[^a-zA-Z0-9]").WithMessage("Password must contain at least one special character");
	}
}
