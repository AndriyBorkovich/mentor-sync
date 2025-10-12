namespace MentorSync.Ratings.Domain;

/// <summary>
/// Represents a review of a material by a reviewer
/// </summary>
public sealed class MaterialReview : BaseReview
{
	/// <summary>
	/// Identifier of the material being reviewed
	/// </summary>
	public int MaterialId { get; set; }
	/// <summary>
	/// Identifier of the reviewer who made the review
	/// </summary>
	public int ReviewerId { get; set; }
}
