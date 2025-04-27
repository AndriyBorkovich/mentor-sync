using Ardalis.Result;
using MediatR;

namespace MentorSync.Users.Features.Confirm;

public sealed record ConfirmAccountCommand(string Email, string Token) : IRequest<Result<string>>;
