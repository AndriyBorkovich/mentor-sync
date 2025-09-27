namespace MentorSync.Ratings.Features.MentorReview.Get;

public sealed record GetMentorReviewsQuery(int MentorId) : IQuery<MentorReviewsResponse>;
