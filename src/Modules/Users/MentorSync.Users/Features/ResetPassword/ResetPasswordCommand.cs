
namespace MentorSync.Users.Features.ResetPassword;

/// <summary>
/// Command to reset a user's password
/// </summary>
public sealed record ResetPasswordCommand(
	string Password,
	string ConfirmPassword,
	string Email,
	string Token) : ICommand<string>;
