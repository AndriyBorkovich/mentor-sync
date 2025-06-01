using Ardalis.Result;
using MediatR;

namespace MentorSync.Ratings.Features.MentorReview.Create;

public sealed record CreateMentorReviewCommand(int MentorId, int MenteeId, int Rating, string ReviewText)
    : IRequest<Result<CreateMentorReviewResponse>>;
