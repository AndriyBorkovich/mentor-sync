using System.ComponentModel.DataAnnotations;

namespace MentorSync.Ratings.Features.MentorReview.Update;

/// <summary>
/// Request model for updating a review for a mentor.
/// </summary>
public sealed class UpdateMentorReviewRequest
{
	/// <summary>
	/// The unique identifier of the review being updated.
	/// </summary>
	[Required]
	public int ReviewId { get; set; }

	/// <summary>
	/// The unique identifier of the mentor being reviewed.
	/// </summary>
	[Range(1, 5)]
	[Required]
	public int Rating { get; set; }

	/// <summary>
	/// The review text provided by the reviewer.
	/// </summary>
	[Required]
	[StringLength(1000, MinimumLength = 10)]
	public string ReviewText { get; set; } = string.Empty;
}
