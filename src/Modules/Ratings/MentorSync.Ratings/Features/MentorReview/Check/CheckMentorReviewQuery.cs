namespace MentorSync.Ratings.Features.MentorReview.Check;

public sealed record CheckMentorReviewQuery(int MentorId, int MenteeId)
	: IQuery<CheckMentorReviewResponse>;
