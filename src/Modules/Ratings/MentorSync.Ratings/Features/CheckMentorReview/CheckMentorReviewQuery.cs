using Ardalis.Result;
using MediatR;

namespace MentorSync.Ratings.Features.CheckMentorReview;

public sealed record CheckMentorReviewQuery(int MentorId, int MenteeId)
    : IRequest<Result<CheckMentorReviewResponse>>;
