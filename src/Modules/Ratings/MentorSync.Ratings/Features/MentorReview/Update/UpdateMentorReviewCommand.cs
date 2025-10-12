namespace MentorSync.Ratings.Features.MentorReview.Update;

/// <summary>
/// Command model for updating a mentor review.
/// </summary>
public sealed record UpdateMentorReviewCommand(int ReviewId, int MenteeId, int Rating, string ReviewText)
	: ICommand<UpdateMentorReviewResponse>;
