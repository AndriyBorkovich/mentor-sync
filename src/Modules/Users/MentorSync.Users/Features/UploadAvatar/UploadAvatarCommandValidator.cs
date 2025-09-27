using FluentValidation;

namespace MentorSync.Users.Features.UploadAvatar;

public sealed class UploadAvatarCommandValidator : AbstractValidator<UploadAvatarCommand>
{
	public UploadAvatarCommandValidator()
	{
		RuleFor(x => x.UserId)
		   .NotNull()
		   .GreaterThan(0);

		RuleFor(x => x.File)
			.Cascade(CascadeMode.Stop)
			.Must(f => f != null)
			.WithMessage("File is required.")
			.Must(file => file.Length > 0)
			.WithMessage("File must not be empty.")
			.Must(file => string.Equals(file.ContentType, "image/png", StringComparison.OrdinalIgnoreCase) || string.Equals(file.ContentType, "image/jpeg", StringComparison.OrdinalIgnoreCase))
			.WithMessage("File must be a png or jpeg image.");
	}
}
