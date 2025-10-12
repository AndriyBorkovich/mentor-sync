namespace MentorSync.Users.Features.Common.Responses;

/// <summary>
/// Response model for authentication
/// </summary>
public sealed record AuthResponse(
	string Token,
	string RefreshToken,
	DateTime Expiration,
	bool? NeedOnboarding);
