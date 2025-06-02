namespace MentorSync.Recommendations.Features.GetRecommendedMaterials;

/// <summary>
/// Response model for recommended learning material results
/// </summary>
public sealed record RecommendedMaterialResponse(
    int Id,
    string Title,
    string Description,
    string Type,
    List<string> Tags,
    int MentorId,
    string MentorName,
    DateTime CreatedAt,
    float CollaborativeScore,
    float ContentBasedScore,
    float FinalScore);
