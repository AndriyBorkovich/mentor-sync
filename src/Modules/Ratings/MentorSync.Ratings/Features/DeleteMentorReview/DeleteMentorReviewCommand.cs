using Ardalis.Result;
using MediatR;

namespace MentorSync.Ratings.Features.DeleteMentorReview;

public sealed record DeleteMentorReviewCommand(int ReviewId, int MenteeId)
    : IRequest<Result>;
