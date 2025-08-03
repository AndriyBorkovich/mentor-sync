namespace MentorSync.Ratings.Features.MaterialReview.Update;

public sealed record UpdateMaterialReviewCommand(int ReviewId, int ReviewerId, int Rating, string ReviewText)
	: ICommand<string>;
