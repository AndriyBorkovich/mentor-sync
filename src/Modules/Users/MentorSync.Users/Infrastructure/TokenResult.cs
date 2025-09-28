namespace MentorSync.Users.Infrastructure;

/// <summary>
/// Represents the result of JWT token generation containing access token, refresh token, and expiration
/// </summary>
/// <param name="AccessToken">The JWT access token</param>
/// <param name="RefreshToken">The JWT refresh token</param>
/// <param name="Expiration">The expiration date and time of the access token</param>
public sealed record TokenResult(
	string AccessToken,
	string RefreshToken,
	DateTime Expiration);