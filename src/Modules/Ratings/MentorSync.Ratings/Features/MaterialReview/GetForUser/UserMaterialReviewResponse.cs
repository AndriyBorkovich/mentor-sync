namespace MentorSync.Ratings.Features.MaterialReview.GetForUser;

/// <summary>
/// Response model for a user's review of a specific learning material
/// </summary>
public sealed record UserMaterialReviewResponse(
	int ReviewId,
	int Rating,
	string ReviewText,
	DateTime CreatedAt,
	DateTime? UpdatedAt);
