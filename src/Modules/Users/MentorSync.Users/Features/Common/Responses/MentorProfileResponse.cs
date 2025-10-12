namespace MentorSync.Users.Features.Common.Responses;

/// <summary>
///	 Response model for mentor profile
/// </summary>
public sealed record MentorProfileResponse(int Id,
										   Industry Industries,
										   IReadOnlyList<string> Skills,
										   IReadOnlyList<string> ProgrammingLanguages,
										   int ExperienceYears,
										   string Availability,
										   int MentorId);
