using Ardalis.Result;
using MentorSync.Notifications.Contracts.Models;
using MentorSync.Users.Domain.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;

namespace MentorSync.Users.Features.ForgotPassword;

/// <summary>
/// Handler for <see cref="ForgotPasswordCommand"/>
/// </summary>
/// <param name="userManager">User manager</param>
/// <param name="mediator">Mediator for sending commands</param>
/// <param name="logger">Logger instance</param>
public sealed class ForgotPasswordCommandHandler(
	UserManager<AppUser> userManager,
	IMediator mediator,
	ILogger<ForgotPasswordCommandHandler> logger)
	: ICommandHandler<ForgotPasswordCommand, string>
{
	/// <inheritdoc />
	public async Task<Result<string>> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken = default)
	{
		var user = await userManager.FindByEmailAsync(request.Email);
		if (user == null)
		{
			return Result.NotFound("User not found");
		}

		var token = await userManager.GeneratePasswordResetTokenAsync(user);
		var @params = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
		{
			{"token", token },
			{"email", user.Email },
		};
		var callback = QueryHelpers.AddQueryString($"{request.BaseUrl}/users/reset-password", @params);

		var emailCommand = new SendEmailCommand
		{
			From = GeneralConstants.DefaultEmail,
			To = user.Email,
			Subject = "Reset password",
			Body = $"Hi! Please reset your password by clicking on link below.\n {callback}",
		};

		var result = await mediator.SendCommandAsync<SendEmailCommand, string>(emailCommand, cancellationToken);

		logger.LogInformation("Password reset email was {Result} to user {UserId}", result.IsSuccess ? "sent" : "not sent", user.Id);

		return result.IsSuccess
			? Result.Success("Password reset email sent")
			: Result.Conflict("Failed to send password reset email");
	}
}
