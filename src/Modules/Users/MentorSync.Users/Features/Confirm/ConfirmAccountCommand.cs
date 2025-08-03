namespace MentorSync.Users.Features.Confirm;

public sealed record ConfirmAccountCommand(string Email, string Token) : ICommand<string>;
