namespace MentorSync.Ratings.Features.MaterialReview.Create;

public sealed record CreateMaterialReviewCommand(int MaterialId, int ReviewerId, int Rating, string ReviewText)
	: ICommand<CreateMaterialReviewResponse>;
