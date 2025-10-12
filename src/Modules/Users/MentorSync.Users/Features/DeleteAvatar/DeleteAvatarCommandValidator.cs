using FluentValidation;

namespace MentorSync.Users.Features.DeleteAvatar;

/// <summary>
/// Validator for <see cref="DeleteAvatarCommand"/>
/// </summary>
public sealed class DeleteAvatarCommandValidator : AbstractValidator<DeleteAvatarCommand>
{
	/// <summary>
	/// Initializes a new instance of the <see cref="DeleteAvatarCommandValidator"/> class.
	/// </summary>
	public DeleteAvatarCommandValidator()
	{
		RuleFor(c => c.UserId)
			.NotNull()
			.GreaterThan(0);
	}
}
