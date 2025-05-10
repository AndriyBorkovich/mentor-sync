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
            .Must(file => file.ContentType == "image/png" || file.ContentType == "image/jpeg")
            .WithMessage("File must be a png or jpeg image.");
    }
}
