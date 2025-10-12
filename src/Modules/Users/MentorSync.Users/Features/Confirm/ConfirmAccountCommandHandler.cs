using Ardalis.Result;
using MentorSync.Users.Domain.User;
using MentorSync.Users.Extensions;
using Microsoft.AspNetCore.Identity;

namespace MentorSync.Users.Features.Confirm;

/// <summary>
/// Command handler for confirming a user's account
/// </summary>
/// <param name="userManager">Identity manager</param>
public sealed class ConfirmAccountCommandHandler(
	UserManager<AppUser> userManager)
	: ICommandHandler<ConfirmAccountCommand, string>
{
	/// <inheritdoc />
	public async Task<Result<string>> Handle(ConfirmAccountCommand request, CancellationToken cancellationToken = default)
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
