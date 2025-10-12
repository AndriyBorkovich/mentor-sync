using Microsoft.AspNetCore.Http;

namespace MentorSync.Users.Features.UploadAvatar;

/// <summary>
/// Command to upload a user's avatar
/// </summary>
public sealed record UploadAvatarCommand(int UserId, IFormFile File) : ICommand<string>;

