using MentorSync.SharedKernel.Abstractions.Messaging;

namespace MentorSync.Users.Features.DeleteAvatar;

public sealed record class DeleteAvatarCommand(int UserId) : ICommand<string>;
