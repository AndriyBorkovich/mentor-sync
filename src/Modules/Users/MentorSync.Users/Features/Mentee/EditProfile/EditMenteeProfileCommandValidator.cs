using FluentValidation;

namespace MentorSync.Users.Features.Mentee.EditProfile;

/// <summary>
/// Validator for <see cref="EditMenteeProfileCommand"/>
/// </summary>
public sealed class EditMenteeProfileCommandValidator : AbstractValidator<EditMenteeProfileCommand>
{
	/// <summary>
	/// Initializes a new instance of the <see cref="EditMenteeProfileCommandValidator"/> class.
	/// </summary>
	public EditMenteeProfileCommandValidator()
	{
		RuleFor(x => x.Id).GreaterThan(0);

		RuleFor(x => x.Bio)
			.NotEmpty()
			.MaximumLength(2000);

		RuleFor(x => x.Position)
			.NotEmpty()
			.MaximumLength(100);

		RuleFor(x => x.Company)
			.NotEmpty()
			.MaximumLength(100);

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

		RuleFor(x => x.MenteeId)
			.NotNull()
			.GreaterThan(0);
	}
}
