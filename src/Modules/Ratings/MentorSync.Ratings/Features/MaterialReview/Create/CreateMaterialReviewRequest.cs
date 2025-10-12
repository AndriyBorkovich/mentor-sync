namespace MentorSync.Ratings.Features.MaterialReview.Create;

/// <summary>
/// Request model for creating a material review.
/// </summary>
/// <param name="ReviewerId">The unique identifier of the reviewer.</param>
/// <param name="Rating">The rating value given to the material.</param>
/// <param name="ReviewText">The review text provided by the reviewer.</param>
/// <example>
/// <code>
/// var request = new CreateMaterialReviewRequest(42, 5, "Excellent material!");
/// </code>
/// </example>
public sealed record CreateMaterialReviewRequest(int ReviewerId, int Rating, string ReviewText);
