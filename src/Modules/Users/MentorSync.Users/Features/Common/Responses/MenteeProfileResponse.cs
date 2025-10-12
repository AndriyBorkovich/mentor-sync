namespace MentorSync.Users.Features.Common.Responses;

/// <summary>
/// Response model for mentee profile
/// </summary>
public sealed record MenteeProfileResponse(int Id,
										   string Bio,
										   string Position,
										   string Company,
										   Industry Industries,
										   IReadOnlyList<string> Skills,
										   IReadOnlyList<string> ProgrammingLanguages,
										   IReadOnlyList<string> LearningGoals,
										   int MenteeId);
