using FluentValidation;

namespace MentorSync.Users.Features.Register;

/// <summary>
/// Validates the user registration command to ensure all required fields are properly formatted.
/// </summary>
/// <remarks>
/// This validator ensures: <br/>
/// - Email is properly formatted and has valid length <br/>
/// - Username meets minimum and maximum length requirements <br/>
/// - Role is one of the predefined system roles <br/>
/// - Password meets security requirements (length, character types) <br/>
/// - Confirmation password matches the original password <br/>
/// </remarks>
public sealed class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
	/// <summary>
	/// Initializes a new instance of the <see cref="RegisterCommandValidator"/> class
	/// and configures all validation rules for the user registration process.
	/// </summary>
	public RegisterCommandValidator()
	{
		RuleFor(x => x.Email)
			.NotNull()
			.NotEmpty()
			.EmailAddress()
			.MaximumLength(GeneralConstants.MaxEmailLength);

		RuleFor(x => x.UserName)
			.NotNull()
			.NotEmpty()
			.MinimumLength(4)
			.MaximumLength(50);

		RuleFor(x => x.Role)
			.NotNull()
			.NotEmpty()
			.Must(x => x.Equals(Roles.Admin, StringComparison.InvariantCultureIgnoreCase)
						|| x.Equals(Roles.Mentor, StringComparison.InvariantCultureIgnoreCase)
						|| x.Equals(Roles.Mentee, StringComparison.InvariantCultureIgnoreCase))
			.WithMessage("Such role doesn't exist");

		ApplyPasswordValidationRules(RuleFor(x => x.Password));

		ApplyPasswordValidationRules(RuleFor(x => x.ConfirmPassword))
			.Equal(x => x.Password)
			.WithMessage("Passwords do not match");
	}

	/// <summary>
	/// Applies common password validation rules to a rule builder
	/// </summary>
	/// <param name="ruleBuilder">The rule builder to apply password validation rules to</param>
	/// <returns>The modified rule builder with password validation rules applied</returns>
	private static IRuleBuilder<RegisterCommand, string> ApplyPasswordValidationRules(
		IRuleBuilder<RegisterCommand, string> ruleBuilder)
	{
		return ruleBuilder
			.NotNull()
			.NotEmpty()
			.MinimumLength(GeneralConstants.MinPasswordLength)
			.Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter")
			.Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter")
			.Matches("[0-9]").WithMessage("Password must contain at least one number")
			.Matches("[^a-zA-Z0-9]").WithMessage("Password must contain at least one special character");
	}
}
