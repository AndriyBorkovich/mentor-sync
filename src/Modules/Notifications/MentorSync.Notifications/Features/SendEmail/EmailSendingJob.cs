using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace MentorSync.Notifications.Features.SendEmail;

public class EmailSendingJob(ILogger<EmailSendingJob> logger, IOutboxProcessor outboxProcessor) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        const int delayMilliseconds = 30_000; // 30 seconds
        logger.LogInformation("{serviceName} starting.", nameof(EmailSendingJob));
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await outboxProcessor.CheckForEmailsToSend();
            }
            catch (Exception ex)
            {
                logger.LogError("Error processing outbox: {message}", ex.Message);
            }
            finally
            {
                await Task.Delay(delayMilliseconds, stoppingToken);
            }
        }

        logger.LogInformation("{serviceName} stopping.", nameof(EmailSendingJob));
    }
}
