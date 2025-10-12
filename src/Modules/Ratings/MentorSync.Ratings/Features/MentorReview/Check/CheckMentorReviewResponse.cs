namespace MentorSync.Ratings.Features.MentorReview.Check;

/// <summary>
/// Response model indicating whether a user has reviewed a specific mentor
/// </summary>
public sealed record CheckMentorReviewResponse
{
	/// <summary>
	/// Indicates if the user has already reviewed the mentor
	/// </summary>
	public bool HasReviewed { get; set; }
	/// <summary>
	/// The ID of the existing review if one exists, otherwise null
	/// </summary>
	public int? ReviewId { get; set; }
	/// <summary>
	/// The rating given by the user in the existing review, if any (1-5 scale)
	/// </summary>
	public int? Rating { get; set; }
	/// <summary>
	/// The textual review provided by the user, if any
	/// </summary>
	public string ReviewText { get; set; }
}
