using System.Diagnostics;
using MentorSync.Notifications.Data;
using MentorSync.Notifications.Domain;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace MentorSync.Notifications.Features.SendEmail;

public class MongoDbEmailOutboxProcessor : IOutboxProcessor
{
    private readonly IMongoCollection<EmailOutbox> _emailEntityCollection;
    private readonly IEmailSender _emailSender;
    private readonly ILogger<MongoDbEmailOutboxProcessor> _logger;

    public MongoDbEmailOutboxProcessor(
        IMongoClient mongoClient,
        IOptionsMonitor<MongoSettings> settings,
        IEmailSender emailSender,
        ILogger<MongoDbEmailOutboxProcessor> logger)
    {
        var database = mongoClient.GetDatabase(settings.CurrentValue.Database);
        _emailEntityCollection = database.GetCollection<EmailOutbox>(settings.CurrentValue.Collection);
        _emailSender = emailSender;
        _logger = logger;
    }

    /// <summary>
    /// Checks for any emails that haven't been sent yet and sends them
    /// </summary>
    /// <returns></returns>
    public async Task CheckForEmailsToSend()
    {
        var filter = Builders<EmailOutbox>.Filter.Eq(entity => entity.DateTimeUtcProcessed, null);

        var unsentEmails = await _emailEntityCollection.Find(filter).ToListAsync();

        if (unsentEmails.Count == 0)
        {
            _logger.LogInformation("No emails to send, sleeping...");
            return;
        }

        foreach (var unsentEmailEntity in unsentEmails)
        {
            try
            {
                _logger.LogInformation("Sending email {id}", unsentEmailEntity.Id);

                await _emailSender.SendAsync(
                    unsentEmailEntity.To,
                    unsentEmailEntity.From,
                    unsentEmailEntity.Subject,
                    unsentEmailEntity.Body
                );

                var stopwatch = Stopwatch.StartNew();
                var updateFilter = Builders<EmailOutbox>.Filter.Eq(x => x.Id, unsentEmailEntity.Id);
                var update = Builders<EmailOutbox>.Update.Set(x => x.DateTimeUtcProcessed, DateTime.UtcNow);
                var updateResult = await _emailEntityCollection.UpdateOneAsync(updateFilter, update);
                stopwatch.Stop();

                _logger.LogInformation("Email {id} processed. {result} record(s) updated in {time}ms",
                    unsentEmailEntity.Id,
                    updateResult.ModifiedCount,
                    stopwatch.Elapsed.TotalMilliseconds);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send email {id}: {message}", unsentEmailEntity.Id, ex.Message);
            }
        }
    }
}
