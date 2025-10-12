using Ardalis.Result;
using MentorSync.Users.Domain.User;
using MentorSync.Users.Extensions;
using Microsoft.AspNetCore.Identity;

namespace MentorSync.Users.Features.ResetPassword;

/// <summary>
/// Command handler for resetting a user's password
/// </summary>
/// <param name="userManager">User manager</param>
public sealed class ResetPasswordCommandHandler(
	UserManager<AppUser> userManager) : ICommandHandler<ResetPasswordCommand, string>
{
	/// <inheritdoc />
	public async Task<Result<string>> Handle(ResetPasswordCommand request, CancellationToken cancellationToken = default)
	{
		var user = await userManager.FindByEmailAsync(request.Email);
		if (user == null)
		{
			return Result.NotFound("User not found");
		}

		var decodedToken = Uri.UnescapeDataString(request.Token);
		var resetResult = await userManager.ResetPasswordAsync(user, decodedToken, request.Password);
		return !resetResult.Succeeded
			? Result.Conflict($"Failed to reset password : {resetResult.GetErrorMessage()}")
			: Result.Success("Password was reset successfully");
	}
}
