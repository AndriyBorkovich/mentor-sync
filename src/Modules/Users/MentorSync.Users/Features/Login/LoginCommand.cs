using MentorSync.Users.Features.Common.Responses;

namespace MentorSync.Users.Features.Login;

/// <summary>
/// Command to log in a user
/// </summary>
public sealed record LoginCommand : ICommand<AuthResponse>
{
	/// <summary>
	/// User's email address
	/// </summary>
	public string Email { get; init; } = string.Empty;
	/// <summary>
	/// User's password
	/// </summary>
	public string Password { get; init; } = string.Empty;
}
