using Ardalis.Result;
using MediatR;

namespace MentorSync.Users.Features.ForgotPassword;

public sealed record ForgotPasswordCommand(string Email, string BaseUrl) : IRequest<Result<string>>;
