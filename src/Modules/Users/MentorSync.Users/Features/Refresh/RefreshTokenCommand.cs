using MentorSync.Users.Features.Common.Responses;

namespace MentorSync.Users.Features.Refresh;

/// <summary>
/// Command to refresh authentication tokens
/// </summary>
public sealed record RefreshTokenCommand : ICommand<AuthResponse>
{
	/// <summary>
	/// The access token to be refreshed
	/// </summary>
	public string AccessToken { get; init; } = string.Empty;
	/// <summary>
	/// The refresh token to be used for obtaining new tokens
	/// </summary>
	public string RefreshToken { get; init; } = string.Empty;
}
