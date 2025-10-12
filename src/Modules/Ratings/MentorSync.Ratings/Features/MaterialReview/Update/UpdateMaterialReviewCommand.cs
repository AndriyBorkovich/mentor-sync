namespace MentorSync.Ratings.Features.MaterialReview.Update;

/// <summary>
/// Command model for updating a material review.
/// </summary>
public sealed record UpdateMaterialReviewCommand(int ReviewId, int ReviewerId, int Rating, string ReviewText)
	: ICommand<string>;
