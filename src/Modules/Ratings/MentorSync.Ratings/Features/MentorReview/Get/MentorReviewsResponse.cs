namespace MentorSync.Ratings.Features.MentorReview.Get;

/// <summary>
/// Response model for retrieving mentor reviews.
/// </summary>
public sealed record MentorReviewsResponse
{
	/// <summary>
	/// Total number of reviews for the mentor.
	/// </summary>
	public int ReviewCount { get; init; }
	/// <summary>
	/// Reviews for the mentor.
	/// </summary>
	public IReadOnlyList<MentorReview> Reviews { get; init; } = [];
}

/// <summary>
/// Model representing a single mentor review.
/// </summary>
public sealed record MentorReview
{
	/// <summary>
	/// Unique identifier for the review.
	/// </summary>
	public int Id { get; init; }
	/// <summary>
	/// Name of the reviewer (mentee).
	/// </summary>
	public string ReviewerName { get; init; }
	/// <summary>
	/// URL of the reviewer's profile image.
	/// </summary>
	public string ReviewerImage { get; init; }
	/// <summary>
	/// Rating given in the review (1-5 scale).
	/// </summary>
	public int Rating { get; init; }
	/// <summary>
	/// Textual comment provided in the review.
	/// </summary>
	public string Comment { get; init; }
	/// <summary>
	/// Timestamp when the review was created.
	/// </summary>
	public DateTime CreatedOn { get; init; }
}
