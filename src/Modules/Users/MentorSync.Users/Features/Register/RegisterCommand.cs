using Ardalis.Result;
using MediatR;

namespace MentorSync.Users.Features.Register;

/// <summary>
/// Command containing user credentials
/// </summary>
/// <param name="Email"></param>
/// <param name="UserName"></param>
/// <param name="Password"></param>
/// <param name="ConfirmPassword"></param>
public sealed record RegisterCommand(
    string Email,
    string UserName,
    string Password,
    string ConfirmPassword) : IRequest<Result>;