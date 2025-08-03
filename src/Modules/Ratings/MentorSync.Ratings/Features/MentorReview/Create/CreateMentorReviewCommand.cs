namespace MentorSync.Ratings.Features.MentorReview.Create;

public sealed record CreateMentorReviewCommand(int MentorId, int MenteeId, int Rating, string ReviewText)
	: ICommand<CreateMentorReviewResponse>;
