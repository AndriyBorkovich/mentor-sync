using Ardalis.Result;
using MediatR;
using MentorSync.Recommendations.Data;
using MentorSync.Recommendations.Domain.Tracking;
using Microsoft.Extensions.Logging;

namespace MentorSync.Recommendations.Features.CreateMentorViewEvent;

public sealed class CreateMentorViewEventCommandHandler(
    RecommendationsDbContext dbContext,
    ILogger<CreateMentorViewEventCommandHandler> logger)
        : IRequestHandler<CreateMentorViewEventCommand, Result<Unit>>
{
    public async Task<Result<Unit>> Handle(CreateMentorViewEventCommand request, CancellationToken cancellationToken)
    {
        var viewEvent = new MentorViewEvent
        {
            MenteeId = request.MenteeId,
            MentorId = request.MentorId,
            ViewedAt = DateTime.UtcNow
        };

        dbContext.MentorViewEvents.Add(viewEvent);
        await dbContext.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Created mentor view event for Mentee {MenteeId} and Mentor {MentorId}", request.MenteeId, request.MentorId);

        return Unit.Value;
    }
}
