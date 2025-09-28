using MentorSync.Notifications.Contracts.Models;
using MentorSync.SharedKernel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace MentorSync.Users.Domain.User;

public sealed class UserCreatedEventHandler(
	IServiceProvider serviceProvider,
	IMediator mediator,
	ILogger<UserCreatedEventHandler> logger) : INotificationHandler<UserCreatedEvent>
{
	public async Task HandleAsync(UserCreatedEvent notification, CancellationToken cancellationToken = default)
	{
		await using var scope = serviceProvider.CreateAsyncScope();
		var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
		var id = notification.UserId;
		var user = await userManager.Users.FirstOrDefaultAsync(u => u.Id == id, cancellationToken: cancellationToken);
		if (user is null)
		{
			logger.LogWarning("User with {UserId} not found", id);
			return;
		}

		var token = await userManager.GenerateEmailConfirmationTokenAsync(user);

		var @params = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
		{
			{ "token", token },
			{ "email", user.Email },
		};
		var callback = QueryHelpers.AddQueryString("users/confirm", @params);

		var emailCommand = new SendEmailCommand()
		{
			From = GeneralConstants.DefaultEmail,
			To = user.Email,
			Subject = "Welcome to MentorSync",
			Body = $"Hi! Please confirm your email address below.\n {callback}",
		};

		var result = await mediator.SendCommandAsync<SendEmailCommand, string>(emailCommand, cancellationToken);

		logger.LogInformation("Welcome email was {Result} to user {UserId}", result.IsSuccess ? "sent" : "not sent", id);
	}
}
