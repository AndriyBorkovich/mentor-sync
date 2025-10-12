namespace MentorSync.Ratings.Domain;

/// <summary>
/// Represents a review made by a reviewer on a specific material
/// </summary>
public sealed class MentorReview : BaseReview
{
	/// <summary>
	/// Identifier of the mentor being reviewed
	/// </summary>
	public int MentorId { get; set; }
	/// <summary>
	/// Identifier of the mentee providing the review
	/// </summary>
	public int MenteeId { get; set; }
}
