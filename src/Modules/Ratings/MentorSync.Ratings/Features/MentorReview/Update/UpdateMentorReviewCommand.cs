using Ardalis.Result;
using MediatR;

namespace MentorSync.Ratings.Features.MentorReview.Update;

public sealed record UpdateMentorReviewCommand(int ReviewId, int MenteeId, int Rating, string ReviewText)
    : IRequest<Result<UpdateMentorReviewResponse>>;
