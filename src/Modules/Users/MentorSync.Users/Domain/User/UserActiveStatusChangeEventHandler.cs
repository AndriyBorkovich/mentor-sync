using MentorSync.Notifications.Contracts.Models;
using Microsoft.Extensions.Logging;

namespace MentorSync.Users.Domain.User;

/// <summary>
/// Handles the event when a user's active status changes by sending an email notification.
/// </summary>
/// <param name="mediator">Mediator</param>
/// <param name="logger">Logger</param>
public sealed class UserActiveStatusChangeEventHandler(
	IMediator mediator,
	ILogger<UserActiveStatusChangeEventHandler> logger)
	: INotificationHandler<UserActiveStatusChangedEvent>
{
	/// <inheritdoc />
	public async Task HandleAsync(UserActiveStatusChangedEvent notification, CancellationToken cancellationToken = default)
	{
		var emailCommand = new SendEmailCommand()
		{
			From = GeneralConstants.DefaultEmail,
			To = notification.Email,
			Subject = "Account status in MentorSync",
			Body = $"""
                    Hi! Please be informed that your account was {(notification.IsActive ? "activated" : "deactivated")}.
                    Please contact support if you have any questions.
                    """,
		};

		var result = await mediator.SendCommandAsync<SendEmailCommand, string>(emailCommand, cancellationToken);

		logger.LogInformation("User status email was {Result} to mail {UserId}", result.IsSuccess ? "sent" : "not sent", notification.Email);
	}
}
