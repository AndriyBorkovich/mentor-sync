using Ardalis.Result;
using MediatR;

namespace MentorSync.Ratings.Features.MentorReview.Get;

public record GetMentorReviewsQuery(int MentorId) : IRequest<Result<MentorReviewsResponse>>;
