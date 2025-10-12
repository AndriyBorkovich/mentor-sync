using MentorSync.Users.Domain.Enums;
using MentorSync.Users.Features.Common.Responses;

namespace MentorSync.Users.Features.Mentor.EditProfile;

/// <summary>
/// Command to edit a mentor's profile
/// </summary>
public sealed record EditMentorProfileCommand(
	int Id,
	string Bio,
	string Position,
	string Company,
	Industry Industries,
	IReadOnlyList<string> Skills,
	IReadOnlyList<string> ProgrammingLanguages,
	int ExperienceYears,
	Availability Availability,
	int MentorId
) : ICommand<MentorProfileResponse>;
