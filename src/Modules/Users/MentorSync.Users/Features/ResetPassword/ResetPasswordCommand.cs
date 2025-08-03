
namespace MentorSync.Users.Features.ResetPassword;

public sealed record ResetPasswordCommand(
	string Password,
	string ConfirmPassword,
	string Email,
	string Token) : ICommand<string>;
