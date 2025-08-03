namespace MentorSync.Ratings.Features.MaterialReview.GetForUser;

public sealed record UserMaterialReviewResponse(
	int ReviewId,
	int Rating,
	string ReviewText,
	DateTime CreatedAt,
	DateTime? UpdatedAt);
