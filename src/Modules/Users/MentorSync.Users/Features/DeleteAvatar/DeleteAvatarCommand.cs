using Ardalis.Result;
using MediatR;

namespace MentorSync.Users.Features.DeleteAvatar;

public sealed record class DeleteAvatarCommand(int UserId) : IRequest<Result<string>>;
