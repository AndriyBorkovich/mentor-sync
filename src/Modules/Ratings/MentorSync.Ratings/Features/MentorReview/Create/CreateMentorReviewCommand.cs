namespace MentorSync.Ratings.Features.MentorReview.Create;

/// <summary>
/// Command model for creating a mentor review.
/// </summary>
public sealed record CreateMentorReviewCommand(int MentorId, int MenteeId, int Rating, string ReviewText)
	: ICommand<CreateMentorReviewResponse>;
