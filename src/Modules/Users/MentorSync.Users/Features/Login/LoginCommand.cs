using MentorSync.Users.Features.Common.Responses;

namespace MentorSync.Users.Features.Login;

public sealed record LoginCommand : ICommand<AuthResponse>
{
	public string Email { get; init; } = string.Empty;
	public string Password { get; init; } = string.Empty;
}