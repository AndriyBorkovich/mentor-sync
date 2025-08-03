namespace MentorSync.Ratings.Features.MaterialReview.Get;

public sealed record GetMaterialReviewsQuery(int MaterialId)
	: IQuery<MaterialReviewsResponse>;
