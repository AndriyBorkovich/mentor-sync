using Ardalis.Result;
using MediatR;
using MentorSync.Users.Domain.User;
using MentorSync.Users.Extensions;
using Microsoft.AspNetCore.Identity;

namespace MentorSync.Users.Features.Confirm;

public sealed class ConfirmAccountCommandHandler(
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

        var decodedToken = Uri.UnescapeDataString(request.Token);

        var result = await userManager.ConfirmEmailAsync(user, decodedToken);
        if (result.Succeeded)
        {
            return Result.Success("Account confirmed");
        }

        return Result.Error($"Email confirmation failed: {result.GetErrorMessage()}");
    }
}
