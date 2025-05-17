using FluentValidation;

namespace MentorSync.Users.Features.Mentor.CreateProfile;

public sealed class CreateMentorProfileCommandValidator : AbstractValidator<CreateMentorProfileCommand>
{
    public CreateMentorProfileCommandValidator()
    {
        RuleFor(x => x.MentorId)
            .NotNull()
            .GreaterThan(0);

        RuleFor(x => x.Skills)
            .NotNull()
            .Must(list => list.Count >= 1 && list.Count <= 20)
            .WithMessage("Skills must contain between 1 and 20 items.");

        RuleFor(x => x.ProgrammingLanguages)
            .NotNull()
            .Must(list => list.Count >= 1 && list.Count <= 10)
            .WithMessage("Programming languages must contain between 1 and 10 items.");

        RuleFor(x => x.ExperienceYears)
            .GreaterThanOrEqualTo(1)
            .LessThan(50);
    }
}
