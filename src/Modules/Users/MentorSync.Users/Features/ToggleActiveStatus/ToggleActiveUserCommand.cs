namespace MentorSync.Users.Features.ToggleActiveStatus;

/// <summary>
/// Command to toggle a user's active status
/// </summary>
public sealed record ToggleActiveUserCommand(int UserId) : ICommand<string>;
