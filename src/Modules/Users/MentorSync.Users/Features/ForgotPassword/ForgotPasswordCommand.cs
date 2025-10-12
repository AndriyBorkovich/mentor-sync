namespace MentorSync.Users.Features.ForgotPassword;

/// <summary>
/// Command to initiate the forgot password process by sending a reset link to the user's email
/// </summary>
public sealed record ForgotPasswordCommand(string Email, string BaseUrl) : ICommand<string>;
