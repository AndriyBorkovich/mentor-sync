using MentorSync.Users.Features.Common.Responses;

namespace MentorSync.Users.Features.Mentee.EditProfile;

/// <summary>
/// Command to edit a mentee profile
/// </summary>
public sealed record EditMenteeProfileCommand(
	int Id,
	string Bio,
	string Position,
	string Company,
	Industry Industries,
	IReadOnlyList<string> Skills,
	IReadOnlyList<string> ProgrammingLanguages,
	IReadOnlyList<string> LearningGoals,
	int MenteeId
) : ICommand<MenteeProfileResponse>;
