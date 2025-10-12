using FluentValidation;

namespace MentorSync.Users.Features.ForgotPassword;

/// <summary>
/// Validator for <see cref="ForgotPasswordCommand"/>
/// </summary>
public sealed class ForgotPasswordCommandValidator : AbstractValidator<ForgotPasswordCommand>
{
	/// <summary>
	/// Initializes a new instance of the <see cref="ForgotPasswordCommandValidator"/> class.
	/// </summary>
	public ForgotPasswordCommandValidator()
	{
		RuleFor(c => c.Email)
			.NotNull()
			.NotEmpty()
			.EmailAddress();
	}
}
