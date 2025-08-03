using MentorSync.Notifications.Contracts;
using MentorSync.SharedKernel;
using Microsoft.Extensions.Logging;

namespace MentorSync.Users.Domain.User;

public sealed class UserActiveStatusChangeEventHandler(
	IMediator mediator,
	ILogger<UserActiveStatusChangeEventHandler> logger)
	: INotificationHandler<UserActiveStatusChageEvent>
{
	public async Task HandleAsync(UserActiveStatusChageEvent notification, CancellationToken cancellationToken)
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
