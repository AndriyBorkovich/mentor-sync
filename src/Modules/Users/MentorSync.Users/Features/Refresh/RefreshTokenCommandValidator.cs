using FluentValidation;

namespace MentorSync.Users.Features.Refresh;

/// <summary>
/// Validator for <see cref="RefreshTokenCommand"/>
/// </summary>
public sealed class RefreshTokenCommandValidator : AbstractValidator<RefreshTokenCommand>
{
	/// <summary>
	/// Initializes a new instance of the <see cref="RefreshTokenCommandValidator"/> class.
	/// </summary>
	public RefreshTokenCommandValidator()
	{
		RuleFor(x => x.AccessToken).NotNull().NotEmpty();
		RuleFor(x => x.RefreshToken).NotNull().NotEmpty();
	}
}
