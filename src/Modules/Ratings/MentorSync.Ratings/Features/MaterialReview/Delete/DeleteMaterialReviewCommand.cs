namespace MentorSync.Ratings.Features.MaterialReview.Delete;

/// <summary>
/// Command to delete a material review
/// </summary>
/// <param name="ReviewId">Identifier of the review to be deleted</param>
/// <param name="UserId">Identifier of the user requesting the deletion</param>
public sealed record DeleteMaterialReviewCommand(int ReviewId, int UserId)
	: ICommand<string>;
