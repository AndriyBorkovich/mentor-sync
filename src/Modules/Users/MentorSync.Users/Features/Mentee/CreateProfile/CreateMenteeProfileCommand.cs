using MentorSync.Users.Features.Common.Responses;

namespace MentorSync.Users.Features.Mentee.CreateProfile;

/// <summary>
/// Command to create a mentee profile
/// </summary>
public sealed record CreateMenteeProfileCommand(
	string Bio,
	string Position,
	string Company,
	Industry Industries,
	IReadOnlyList<string> Skills,
	IReadOnlyList<string> ProgrammingLanguages,
	IReadOnlyList<string> LearningGoals,
	int MenteeId
) : ICommand<MenteeProfileResponse>;
