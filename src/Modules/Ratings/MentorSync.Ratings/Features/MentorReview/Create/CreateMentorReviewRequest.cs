using System.ComponentModel.DataAnnotations;

namespace MentorSync.Ratings.Features.MentorReview.Create;

/// <summary>
/// Request model for creating a review for a mentor.
/// </summary>
public sealed class CreateMentorReviewRequest
{
	/// <summary>
	/// The unique identifier of the mentor being reviewed.
	/// </summary>
	[Required]
	public int MentorId { get; set; }

	/// <summary>
	/// The unique identifier of the reviewer.
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
