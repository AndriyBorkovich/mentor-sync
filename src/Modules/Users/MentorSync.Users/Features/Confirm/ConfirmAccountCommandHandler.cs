using Ardalis.Result;
using MediatR;
using MentorSync.Users.Domain;
using MentorSync.Users.Domain.User;
using Microsoft.AspNetCore.Identity;

namespace MentorSync.Users.Features.Confirm;

public class ConfirmAccountCommandHandler(
    UserManager<AppUser> userManager)
    : IRequestHandler<ConfirmAccountCommand, Result<string>>
{
    public async Task<Result<string>> Handle(ConfirmAccountCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByEmailAsync(request.Email);
        if (user is null)
        {
            return Result.NotFound($"User with email {request.Email} not found");
        }

        if (user.EmailConfirmed)
        {
            return Result.Conflict($"User with email {request.Email} already confirmed it");
        }

        var result = await userManager.ConfirmEmailAsync(user, request.Email);
        if (result.Succeeded)
        {
            return Result.Success("Account confirmed");
        }

        var errors = result.Errors.Select(e => e.Description);
        return Result.Error($"Email confirmation failed: {string.Join(", ", errors)}");
    }
}
