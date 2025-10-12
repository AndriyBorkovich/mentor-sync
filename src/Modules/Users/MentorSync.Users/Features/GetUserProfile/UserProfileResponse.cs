namespace MentorSync.Users.Features.GetUserProfile;

/// <summary>
/// Response model for user profile
/// </summary>
public sealed record UserProfileResponse(
	int Id,
	string UserName,
	string Email,
	string Role,
	string ProfileImageUrl,
	bool IsActive,
	MenteeProfileInfo MenteeProfile,
	MentorProfileInfo MentorProfile
);

/// <summary>
/// Mentee profile information
/// </summary>
public sealed record MenteeProfileInfo(
	int Id,
	string Bio,
	string Position,
	string Company,
	string Category,
	IReadOnlyList<string> Skills,
	IReadOnlyList<string> ProgrammingLanguages,
	IReadOnlyList<string> LearningGoals
);

/// <summary>
/// Mentor profile information
/// </summary>
public sealed record MentorProfileInfo(
	int Id,
	string Bio,
	string Position,
	string Company,
	string Category,
	IReadOnlyList<string> Skills,
	IReadOnlyList<string> ProgrammingLanguages,
	int ExperienceYears
);
