namespace MentorSync.Ratings.Contracts.Models;

/// <summary>
/// Model representing the result of a mentor review from a mentee
/// </summary>
public sealed class MentorReviewResult
{
	/// <summary>
	/// The ID of the mentor being reviewed
	/// </summary>
	public int MentorId { get; set; }
	/// <summary>
	/// The ID of the mentee providing the review
	/// </summary>
	public int MenteeId { get; set; }
	/// <summary>
	/// The rating given by the mentee to the mentor (1-5 scale)
	/// </summary>
	public int Rating { get; set; }
}
