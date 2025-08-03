using FluentValidation;

namespace MentorSync.Users.Features.DeleteAvatar;

public sealed class DeleteAvatarCommandValidator : AbstractValidator<DeleteAvatarCommand>
{
	public DeleteAvatarCommandValidator()
	{
		RuleFor(c => c.UserId)
			.NotNull()
			.GreaterThan(0);
	}
}
