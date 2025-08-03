namespace MentorSync.Ratings.Contracts.Models;

/// <summary>
/// Model for mentee ratings of learning materials.
/// </summary>
public sealed class MaterialReviewResult
{
	public int MenteeId { get; set; }
	public int MaterialId { get; set; }
	public int Rating { get; set; }
}
