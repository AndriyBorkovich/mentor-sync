﻿using FluentValidation;

namespace MentorSync.Users.Features.Mentor.EditProfile;

public class EditMentorProfileCommandValidator : AbstractValidator<EditMentorProfileCommand>
{
    public EditMentorProfileCommandValidator()
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
           .Must(list => list.Count >= 1 && list.Count <= 20)
           .WithMessage("Skills must contain between 1 and 20 items.");

        RuleFor(x => x.ProgrammingLanguages)
            .NotNull()
            .Must(list => list.Count >= 1 && list.Count <= 10)
            .WithMessage("Programming languages must contain between 1 and 10 items.");

        RuleFor(x => x.ExperienceYears)
            .GreaterThanOrEqualTo(1)
            .LessThan(50);

        RuleFor(x => x.MentorId)
            .NotNull()
            .GreaterThan(0);
    }
}
