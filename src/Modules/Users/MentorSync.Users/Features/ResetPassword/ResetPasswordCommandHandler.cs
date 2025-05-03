using Ardalis.Result;
using MediatR;
using MentorSync.Users.Domain.User;
using MentorSync.Users.Extensions;
using Microsoft.AspNetCore.Identity;

namespace MentorSync.Users.Features.ResetPassword;

public class ResetPasswordCommandHandler(
    UserManager<AppUser> userManager) : IRequestHandler<ResetPasswordCommand, Result<string>>
{
    public async Task<Result<string>> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByEmailAsync(request.Email);
        if (user == null)
        {
            return Result.NotFound("User not found");
        }

        var decodedToken = Uri.UnescapeDataString(request.Token);
        var resetResult = await userManager.ResetPasswordAsync(user, decodedToken, request.Password);
        if (!resetResult.Succeeded)
        {
            return Result.Conflict($"Failed to reset password : {resetResult.GetErrorMessage()}");
        }

        return Result.Success("Password was reset successfully");
    }
}
