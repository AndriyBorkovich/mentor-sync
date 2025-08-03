using Microsoft.AspNetCore.Http;

namespace MentorSync.Users.Features.UploadAvatar;

public sealed record UploadAvatarCommand(int UserId, IFormFile File) : ICommand<string>;

