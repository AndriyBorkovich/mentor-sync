using MentorSync.Users.Features.Common.Responses;

namespace MentorSync.Users.Features.Mentee.CreateProfile;

public sealed record CreateMenteeProfileCommand(
	string Bio,
	string Position,
	string Company,
	Industry Industries,
	List<string> Skills,
	List<string> ProgrammingLanguages,
	List<string> LearningGoals,
	int MenteeId
) : ICommand<MenteeProfileResponse>;
