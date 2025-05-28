using Ardalis.Result;
using MediatR;

namespace MentorSync.Ratings.Features.UpdateMentorReview;

public sealed record UpdateMentorReviewCommand(int ReviewId, int MenteeId, int Rating, string ReviewText)
    : IRequest<Result<UpdateMentorReviewResponse>>;
