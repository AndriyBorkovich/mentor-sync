using System.Collections.Generic;

namespace MentorSync.Recommendations.Features.GetRecommendedMentors;

/// <summary>
/// Response model for recommended mentor results
/// </summary>
public sealed record RecommendedMentorResponse(
    int Id,
    string Name,
    string Title,
    double Rating,
    List<RecommendedSkillResponse> Skills,
    string ProfileImage,
    int? YearsOfExperience,
    string Category,
    float CollaborativeScore,
    float ContentBasedScore,
    float FinalScore);

/// <summary>
/// Skill response model
/// </summary>
public sealed record RecommendedSkillResponse(
    string Id,
    string Name);
