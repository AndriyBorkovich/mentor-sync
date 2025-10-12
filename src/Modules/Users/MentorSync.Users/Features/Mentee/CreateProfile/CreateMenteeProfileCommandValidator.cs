using FluentValidation;

namespace MentorSync.Users.Features.Mentee.CreateProfile;

/// <summary>
/// Validator for <see cref="CreateMenteeProfileCommand"/>
/// </summary>
public sealed class CreateMenteeProfileCommandValidator : AbstractValidator<CreateMenteeProfileCommand>
{
	/// <summary>
	/// Initializes a new instance of the <see cref="CreateMenteeProfileCommandValidator"/> class.
	/// </summary>
	public CreateMenteeProfileCommandValidator()
	{
		RuleFor(x => x.MenteeId)
			.NotNull()
			.GreaterThan(0);

		RuleFor(x => x.Skills)
			.NotNull()
			.Must(list => list.Count is >= 1 and <= 20)
			.WithMessage("Skills must contain between 1 and 20 items.");

		RuleFor(x => x.ProgrammingLanguages)
			.NotNull()
			.Must(list => list.Count is >= 1 and <= 10)
			.WithMessage("Programming languages must contain between 1 and 10 items.");

		RuleFor(x => x.LearningGoals)
			.NotNull()
			.Must(list => list.Count is >= 1 and <= 5)
			.WithMessage("Learning goals must contain between 1 and 5 items.");
	}
}
