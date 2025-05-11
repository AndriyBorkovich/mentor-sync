using FluentValidation;

namespace MentorSync.Users.Features.Refresh;

public sealed class RefreshTokenCommandValidator : AbstractValidator<RefreshTokenCommand>
{
    public RefreshTokenCommandValidator()
    {
        RuleFor(x => x.AccessToken).NotNull().NotEmpty();
        RuleFor(x => x.RefreshToken).NotNull().NotEmpty();
    }
}
