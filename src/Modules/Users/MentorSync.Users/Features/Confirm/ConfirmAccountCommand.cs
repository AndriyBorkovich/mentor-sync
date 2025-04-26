using Ardalis.Result;
using MediatR;

namespace MentorSync.Users.Features.Confirm;

public record ConfirmAccountCommand(string Email, string Token) : IRequest<Result<string>>;
