namespace MentorSync.Users.Features.GetAllUsers;

/// <summary>
/// Response model for a short representation of a user
/// </summary>
public sealed record UserShortResponse(
	int Id,
	string Name,
	string Email,
	string Role,
	string AvatarUrl,
	bool IsActive,
	bool IsEmailConfirmed);
