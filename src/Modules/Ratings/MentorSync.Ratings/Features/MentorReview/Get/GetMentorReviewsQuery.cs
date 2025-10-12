namespace MentorSync.Ratings.Features.MentorReview.Get;

/// <summary>
/// Query to get all reviews for a specific mentor
/// </summary>
public sealed record GetMentorReviewsQuery(int MentorId) : IQuery<MentorReviewsResponse>;
