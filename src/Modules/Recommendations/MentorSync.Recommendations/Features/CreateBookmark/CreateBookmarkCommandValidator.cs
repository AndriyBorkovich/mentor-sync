using FluentValidation;

namespace MentorSync.Recommendations.Features.CreateBookmark;

public sealed class CreateBookmarkCommandValidator : AbstractValidator<CreateBookmarkCommand>
{
    public CreateBookmarkCommandValidator()
    {
        RuleFor(x => x.MentorId)
            .GreaterThan(0);
        RuleFor(x => x.MenteeId)
            .GreaterThan(0);
    }
}
