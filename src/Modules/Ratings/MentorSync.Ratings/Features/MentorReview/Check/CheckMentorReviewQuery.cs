using Ardalis.Result;
using MediatR;

namespace MentorSync.Ratings.Features.MentorReview.Check;

public sealed record CheckMentorReviewQuery(int MentorId, int MenteeId)
    : IRequest<Result<CheckMentorReviewResponse>>;
