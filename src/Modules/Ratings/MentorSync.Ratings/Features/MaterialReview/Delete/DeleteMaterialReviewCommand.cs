namespace MentorSync.Ratings.Features.MaterialReview.Delete;

public sealed record DeleteMaterialReviewCommand(int ReviewId, int UserId)
	: ICommand<string>;
