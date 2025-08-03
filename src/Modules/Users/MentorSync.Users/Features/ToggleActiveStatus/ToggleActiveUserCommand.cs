namespace MentorSync.Users.Features.ToggleActiveStatus;

public sealed record ToggleActiveUserCommand(int UserId) : ICommand<string>;
