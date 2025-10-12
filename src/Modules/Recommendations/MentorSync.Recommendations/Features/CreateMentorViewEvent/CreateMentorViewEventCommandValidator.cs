using FluentValidation;

namespace MentorSync.Recommendations.Features.CreateMentorViewEvent;

/// <summary>
/// Validator for <see cref="CreateMentorViewEventCommand"/>
/// </summary>
public sealed class CreateMentorViewEventCommandValidator : AbstractValidator<CreateMentorViewEventCommand>
{
	/// <summary>
	/// Initializes a new instance of the <see cref="CreateMentorViewEventCommandValidator"/> class.
	/// </summary>
	public CreateMentorViewEventCommandValidator()
	{
		RuleFor(x => x.MentorId)
			.GreaterThan(0);
		RuleFor(x => x.MenteeId)
			.GreaterThan(0);
	}
}
