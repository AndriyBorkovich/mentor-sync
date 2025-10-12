using MentorSync.Users.Domain.Enums;
using MentorSync.Users.Features.Common.Responses;

namespace MentorSync.Users.Features.Mentor.CreateProfile;

/// <summary>
/// Command to create a mentor profile
/// </summary>
public sealed record CreateMentorProfileCommand(
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
