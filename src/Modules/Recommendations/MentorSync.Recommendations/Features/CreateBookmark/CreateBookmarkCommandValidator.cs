using FluentValidation;

namespace MentorSync.Recommendations.Features.CreateBookmark;

/// <summary>
/// Validator for <see cref="CreateBookmarkCommand"/>
/// </summary>
public sealed class CreateBookmarkCommandValidator : AbstractValidator<CreateBookmarkCommand>
{
	/// <summary>
	/// Initializes a new instance of the <see cref="CreateBookmarkCommandValidator"/> class.
	/// </summary>
	public CreateBookmarkCommandValidator()
	{
		RuleFor(x => x.MentorId)
			.GreaterThan(0);
		RuleFor(x => x.MenteeId)
			.GreaterThan(0);
	}
}
