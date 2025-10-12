using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace MentorSync.Notifications.Features.SendEmail;

/// <summary>
/// Background service that periodically checks the outbox for emails to send
/// </summary>
/// <param name="logger">Logger</param>
/// <param name="outboxProcessor">Outbox event processor service</param>
public sealed class EmailSendingJob(ILogger<EmailSendingJob> logger, IOutboxProcessor outboxProcessor) : BackgroundService
{
	/// <inheritdoc />
	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		const int delayMilliseconds = 30_000; // 30 seconds
		logger.LogInformation("{ServiceName} starting.", nameof(EmailSendingJob));
		while (!stoppingToken.IsCancellationRequested)
		{
			try
			{
				await outboxProcessor.CheckForEmailsToSend();
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "Error processing outbox: {Message}", ex.Message);
			}
			finally
			{
				await Task.Delay(delayMilliseconds, stoppingToken);
			}
		}

		logger.LogInformation("{ServiceName} stopping.", nameof(EmailSendingJob));
	}
}
