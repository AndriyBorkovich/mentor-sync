namespace MentorSync.Users.Features.DeleteAvatar;

/// <summary>
/// Command to delete a user's avatar
/// </summary>
public sealed record DeleteAvatarCommand(int UserId) : ICommand<string>;
