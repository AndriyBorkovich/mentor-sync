using Ardalis.Result;
using MediatR;

namespace MentorSync.Ratings.Features.GetMentorReviews;

public record GetMentorReviewsQuery(int MentorId) : IRequest<Result<MentorReviewsResponse>>;
