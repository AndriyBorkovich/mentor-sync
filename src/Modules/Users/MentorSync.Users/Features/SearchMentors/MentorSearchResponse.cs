namespace MentorSync.Users.Features.SearchMentors;

/// <summary>
/// Response model for mentor search results
/// </summary>
public sealed record MentorSearchResponse(
    int Id,
    string Name,
    string Title,
    double Rating,
    List<SkillResponse> Skills,
    string ProfileImage,
    int? YearsOfExperience,
    string Category);

/// <summary>
/// Skill response model
/// </summary>
public sealed record SkillResponse(
    string Id,
    string Name);
