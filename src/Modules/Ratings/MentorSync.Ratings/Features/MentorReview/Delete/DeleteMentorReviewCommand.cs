using Ardalis.Result;
using MediatR;

namespace MentorSync.Ratings.Features.MentorReview.Delete;

public sealed record DeleteMentorReviewCommand(int ReviewId, int MenteeId)
    : IRequest<Result>;
