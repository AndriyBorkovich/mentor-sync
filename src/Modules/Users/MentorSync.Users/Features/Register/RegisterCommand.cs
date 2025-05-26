using Ardalis.Result;
using MediatR;

namespace MentorSync.Users.Features.Register;

/// <summary>
/// Command containing user credentials and role for registration.
/// </summary>
/// <param name="Email"></param>
/// <param name="UserName"></param>
/// <param name="Role"></param>
/// <param name="Password"></param>
/// <param name="ConfirmPassword"></param>
public sealed record RegisterCommand(
    string Email,
    string UserName,
    string Role,
    string Password,
    string ConfirmPassword) : IRequest<Result<string>>;
