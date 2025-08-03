namespace MentorSync.Users.Features.Common.Responses;

public sealed record AuthResponse(
	string Token,
	string RefreshToken,
	DateTime Expiration,
	bool? NeedOnboarding);
