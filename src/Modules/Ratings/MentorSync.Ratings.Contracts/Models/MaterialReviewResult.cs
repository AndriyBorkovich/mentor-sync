namespace MentorSync.Ratings.Contracts.Models;

/// <summary>
/// Model for mentee ratings of learning materials.
/// </summary>
public sealed class MaterialReviewResult
{
	/// <summary>
	/// The ID of the mentee who provided the rating.
	/// </summary>
	public int MenteeId { get; set; }
	/// <summary>
	/// The ID of the learning material being rated.
	/// </summary>
	public int MaterialId { get; set; }
	/// <summary>
	/// The rating given by the mentee, typically on a scale (e.g., 1 to 5).
	/// </summary>
	public int Rating { get; set; }
}
