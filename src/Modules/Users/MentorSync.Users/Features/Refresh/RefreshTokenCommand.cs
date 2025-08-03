using MentorSync.Users.Features.Common.Responses;

namespace MentorSync.Users.Features.Refresh;

public sealed record RefreshTokenCommand : ICommand<AuthResponse>
{
	public string AccessToken { get; init; } = string.Empty;
	public string RefreshToken { get; init; } = string.Empty;
}