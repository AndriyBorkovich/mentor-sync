using FluentValidation;

namespace MentorSync.Users.Features.ToggleActiveStatus;

/// <summary>
/// Validator for <see cref="ToggleActiveUserCommand"/>
/// </summary>
public sealed class ToggleActiveUserCommandValidator : AbstractValidator<ToggleActiveUserCommand>
{
	/// <summary>
	/// Initializes a new instance of the <see cref="ToggleActiveUserCommandValidator"/> class.
	/// </summary>
	public ToggleActiveUserCommandValidator()
	{
		RuleFor(x => x.UserId)
			.NotNull()
			.GreaterThan(0);
	}
}
