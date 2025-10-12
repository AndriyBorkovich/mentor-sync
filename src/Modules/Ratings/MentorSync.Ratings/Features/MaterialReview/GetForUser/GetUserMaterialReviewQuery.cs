namespace MentorSync.Ratings.Features.MaterialReview.GetForUser;

/// <summary>
/// Query to get a user's review for a specific learning material
/// </summary>
public sealed record GetUserMaterialReviewQuery(int MaterialId, int UserId) : IQuery<UserMaterialReviewResponse>;
