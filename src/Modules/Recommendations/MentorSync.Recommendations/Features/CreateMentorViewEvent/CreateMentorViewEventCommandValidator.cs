using FluentValidation;

namespace MentorSync.Recommendations.Features.CreateMentorViewEvent;

public sealed class CreateMentorViewEventCommandValidator : AbstractValidator<CreateMentorViewEventCommand>
{
    public CreateMentorViewEventCommandValidator()
    {
        RuleFor(x => x.MentorId)
            .GreaterThan(0);
        RuleFor(x => x.MenteeId)
            .GreaterThan(0);
    }
}
