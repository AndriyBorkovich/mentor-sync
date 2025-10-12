namespace MentorSync.Ratings.Features.MaterialReview.Create;

/// <summary>
/// Response returned after creating a material review
/// </summary>
/// <param name="ReviewId">Identifier of created review</param>
public sealed record CreateMaterialReviewResponse(int ReviewId);
