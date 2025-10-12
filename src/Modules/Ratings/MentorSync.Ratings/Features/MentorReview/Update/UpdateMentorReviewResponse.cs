namespace MentorSync.Ratings.Features.MentorReview.Update;

/// <summary>
/// Response model for updating a mentor review
/// </summary>
public sealed class UpdateMentorReviewResponse
{
	/// <summary>
	/// The ID of the updated review
	/// </summary>
	public int ReviewId { get; set; }
	/// <summary>
	/// The ID of the mentor being reviewed
	/// </summary>
	public int MentorId { get; set; }
	/// <summary>
	///	 The ID of the mentee who provided the review
	/// </summary>
	public int MenteeId { get; set; }
	/// <summary>
	/// The updated rating given by the mentee to the mentor (1-5 scale)
	/// </summary>
	public int Rating { get; set; }
	/// <summary>
	/// The updated textual review provided by the mentee
	/// </summary>
	public string ReviewText { get; set; } = string.Empty;
	/// <summary>
	/// The timestamp when the review was last updated
	/// </summary>
	public DateTime UpdatedAt { get; set; }
}
