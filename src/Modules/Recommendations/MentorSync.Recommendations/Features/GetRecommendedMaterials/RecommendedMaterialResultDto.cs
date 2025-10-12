namespace MentorSync.Recommendations.Features.GetRecommendedMaterials;

/// <summary>
/// DTO for SQL query results with recommendations data
/// </summary>
public sealed class RecommendedMaterialResultDto
{
	/// <summary>
	/// Identifier of the recommended material
	/// </summary>
	public int Id { get; set; }
	/// <summary>
	/// Title of the recommended material
	/// </summary>
	public string Title { get; set; }
	/// <summary>
	/// Description of the recommended material
	/// </summary>
	public string Description { get; set; }
	/// <summary>
	/// Type of the recommended material
	/// </summary>
	public string Type { get; set; }
	/// <summary>
	/// Tags associated with the recommended material
	/// </summary>
	public string[] Tags { get; set; }
	/// <summary>
	/// Identifier of the mentor who created the material
	/// </summary>
	public int MentorId { get; set; }
	/// <summary>
	/// Name of the mentor who created the material
	/// </summary>
	public string MentorName { get; set; }
	/// <summary>
	/// Creation date of the recommended material
	/// </summary>
	public DateTime CreatedAt { get; set; }
	/// <summary>
	/// The score from the collaborative filtering algorithm.
	/// </summary>
	public float CollaborativeScore { get; set; }
	/// <summary>
	/// The score from the content-based filtering algorithm.
	/// </summary>
	public float ContentBasedScore { get; set; }
	/// <summary>
	/// The final score is a combination of the collaborative and content-based scores.
	/// </summary>
	public float FinalScore { get; set; }
}
