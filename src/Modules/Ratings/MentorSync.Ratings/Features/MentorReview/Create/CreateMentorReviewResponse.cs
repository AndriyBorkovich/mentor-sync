namespace MentorSync.Ratings.Features.MentorReview.Create;

/// <summary>
/// Response model for creating a mentor review
/// </summary>
public sealed class CreateMentorReviewResponse
{
	/// <summary>
	/// The unique identifier of the created review.
	/// </summary>
	public int ReviewId { get; set; }
	/// <summary>
	/// The unique identifier of the mentor being reviewed.
	/// </summary>
	public int MentorId { get; set; }
	/// <summary>
	/// The unique identifier of the mentee who created the review.
	/// </summary>
	public int MenteeId { get; set; }
	/// <summary>
	/// The rating value given to the mentor.
	/// </summary>
	public int Rating { get; set; }
	/// <summary>
	/// The review text provided by the mentee.
	/// </summary>
	public string ReviewText { get; set; } = string.Empty;
	/// <summary>
	/// The timestamp when the review was created.
	/// </summary>
	public DateTime CreatedAt { get; set; }
}
