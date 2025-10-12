namespace MentorSync.Ratings.Features.MaterialReview.Create;

/// <summary>
/// Command model for creating a material review.
/// </summary>
/// <param name="MaterialId">The unique identifier of the material being reviewed.</param>
/// <param name="ReviewerId">The unique identifier of the reviewer.</param>
/// <param name="Rating">The rating value given to the material.</param>
/// <param name="ReviewText">The review text provided by the reviewer.</param>
public sealed record CreateMaterialReviewCommand(int MaterialId, int ReviewerId, int Rating, string ReviewText)
	: ICommand<CreateMaterialReviewResponse>;
