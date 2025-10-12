namespace MentorSync.Users.Features.Confirm;

/// <summary>
/// Command to confirm a user account using email and token
/// </summary>
public sealed record ConfirmAccountCommand(string Email, string Token) : ICommand<string>;
