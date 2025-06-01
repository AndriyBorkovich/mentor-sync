namespace MentorSync.Recommendations.Features.GetRecommendedMaterials;

/// <summary>
/// DTO for SQL query results with recommendations data
/// </summary>
public sealed class RecommendedMaterialResultDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Type { get; set; }
    public string[] Tags { get; set; }
    public int MentorId { get; set; }
    public string MentorName { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsActive { get; set; }
    public float CollaborativeScore { get; set; }
    public float ContentBasedScore { get; set; }
    public float FinalScore { get; set; }
}
