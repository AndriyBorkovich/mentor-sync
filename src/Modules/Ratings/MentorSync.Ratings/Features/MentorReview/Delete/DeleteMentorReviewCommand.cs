namespace MentorSync.Ratings.Features.MentorReview.Delete;

public sealed record DeleteMentorReviewCommand(int ReviewId, int MenteeId)
	: ICommand<string>;
