using Ardalis.Result;
using MediatR;

namespace MentorSync.Users.Features.ToggleActiveStatus;

public sealed record ToggleActiveUserCommand(int UserId) : IRequest<Result<string>>;
