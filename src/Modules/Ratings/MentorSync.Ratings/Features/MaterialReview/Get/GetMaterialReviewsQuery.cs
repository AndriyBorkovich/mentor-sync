namespace MentorSync.Ratings.Features.MaterialReview.Get;

/// <summary>
/// Query to get all reviews for a specific learning material
/// </summary>
public sealed record GetMaterialReviewsQuery(int MaterialId)
	: IQuery<MaterialReviewsResponse>;
