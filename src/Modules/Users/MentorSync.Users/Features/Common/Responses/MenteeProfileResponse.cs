namespace MentorSync.Users.Features.Common.Responses;

public sealed record MenteeProfileResponse(int Id,
										   string Bio,
										   string Position,
										   string Company,
										   Industry Industries,
										   IReadOnlyList<string> Skills,
										   IReadOnlyList<string> ProgrammingLanguages,
										   IReadOnlyList<string> LearningGoals,
										   int MenteeId);
