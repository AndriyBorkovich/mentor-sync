using Azure;
using Azure.Communication.Email;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace MentorSync.Notifications.Integrations;

public class AzureEmailSender(IConfiguration configuration, ILogger<AzureEmailSender> logger) : IEmailSender
{
    public async Task SendAsync(string to, string from, string subject, string body)
    {
        var connectionString = configuration.GetConnectionString("EmailService");
        var emailClient = new EmailClient(connectionString);

        var emailSendOperation = await emailClient.SendAsync(
            WaitUntil.Started,
            from,
            to,
            subject,
            body);

        try
        {
            while (true)
            {
                await emailSendOperation.UpdateStatusAsync();
                if (emailSendOperation.HasCompleted)
                {
                    break;
                }
                await Task.Delay(100);
            }

            if (emailSendOperation.HasValue)
            {
                logger.LogInformation("Email queued for delivery. Status - {emailStatus}", emailSendOperation.Value.Status);
            }
        }
        catch (RequestFailedException ex)
        {
            logger.LogWarning("Email send failed with code - {ErrorCode} and message - {Message}", ex.ErrorCode, ex.Message);
        }

        logger.LogInformation("Email operation id = {operationId}", emailSendOperation.Id);
    }
}
