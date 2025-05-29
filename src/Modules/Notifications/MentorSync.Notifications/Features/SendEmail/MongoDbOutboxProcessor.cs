using System.Diagnostics;
using MentorSync.Notifications.Data;
using MentorSync.Notifications.Domain;
using MentorSync.Notifications.Infrastructure.Emails;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace MentorSync.Notifications.Features.SendEmail;

public sealed class EmailOutboxProcessor(
    NotificationsDbContext dbContext,
    IEmailSender emailSender,
    ILogger<EmailOutboxProcessor> logger)
    : IOutboxProcessor
{
    /// <summary>
    /// Checks for any emails that haven't been sent yet and sends them
    /// </summary>
    /// <returns></returns>
    public async Task CheckForEmailsToSend()
    {
        var filter = Builders<EmailOutbox>.Filter.Eq(entity => entity.DateTimeUtcProcessed, null);

        var unsentEmails = await dbContext.EmailOutboxes.Find(filter).ToListAsync();

        if (unsentEmails.Count == 0)
        {
            return;
        }

        foreach (var unsentEmailEntity in unsentEmails)
        {
            try
            {
                logger.LogInformation("Sending email {id}", unsentEmailEntity.Id);

                await emailSender.SendAsync(
                    unsentEmailEntity.To,
                    unsentEmailEntity.From,
                    unsentEmailEntity.Subject,
                    unsentEmailEntity.Body
                );

                var stopwatch = Stopwatch.StartNew();
                var updateFilter = Builders<EmailOutbox>.Filter.Eq(x => x.Id, unsentEmailEntity.Id);
                var update = Builders<EmailOutbox>.Update.Set(x => x.DateTimeUtcProcessed, DateTime.UtcNow);
                var updateResult = await dbContext.EmailOutboxes.UpdateOneAsync(updateFilter, update);
                stopwatch.Stop();

                logger.LogInformation("Email {id} processed. {result} record(s) updated in {time}ms",
                    unsentEmailEntity.Id,
                    updateResult.ModifiedCount,
                    stopwatch.Elapsed.TotalMilliseconds);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to send email {id}: {message}", unsentEmailEntity.Id, ex.Message);
            }
        }
    }
}
