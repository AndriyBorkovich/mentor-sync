namespace MentorSync.Ratings.Features.MentorReview.Get;

public record GetMentorReviewsQuery(int MentorId) : IQuery<MentorReviewsResponse>;
