using FluentValidation;

namespace MentorSync.Users.Features.Confirm;

/// <summary>
/// Validator for <see cref="ConfirmAccountCommand"/>
/// </summary>
public sealed class ConfirmAccountValidator : AbstractValidator<ConfirmAccountCommand>
{
	/// <summary>
	/// Initializes a new instance of the <see cref="ConfirmAccountValidator"/> class.
	/// </summary>
	public ConfirmAccountValidator()
	{
		RuleFor(c => c.Email).NotNull().NotEmpty().EmailAddress().MaximumLength(GeneralConstants.MaxEmailLength);
		RuleFor(c => c.Token).NotNull().NotEmpty();
	}
}
