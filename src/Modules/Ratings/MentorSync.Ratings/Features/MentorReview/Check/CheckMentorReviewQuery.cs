namespace MentorSync.Ratings.Features.MentorReview.Check;

/// <summary>
/// Query to check if a mentee has already reviewed a specific mentor
/// </summary>
public sealed record CheckMentorReviewQuery(int MentorId, int MenteeId)
	: IQuery<CheckMentorReviewResponse>;
