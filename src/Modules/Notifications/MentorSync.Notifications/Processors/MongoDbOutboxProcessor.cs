using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Microsoft.Extensions.Options;
using MentorSync.Notifications.Data;
using System.Diagnostics;
using MentorSync.Notifications.Domain;
using MentorSync.Notifications.Integrations;

namespace MentorSync.Notifications.Processors;

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
        var unsentEmailEntity = await _emailEntityCollection.Find(filter).FirstOrDefaultAsync();

        // TODO: Change this to a while loop so it processes more than 1 each time
        if (unsentEmailEntity != null)
        {
            try
            {
                _logger.LogInformation("Sending email {id}", unsentEmailEntity.Id);

                await _emailSender.SendAsync(unsentEmailEntity.To,
                  unsentEmailEntity.From,
                  unsentEmailEntity.Subject,
                  unsentEmailEntity.Body);

                var stopwatch = Stopwatch.StartNew();
                var updateFilter = Builders<EmailOutbox>.Filter.Eq(x => x.Id, unsentEmailEntity.Id);
                var update = Builders<EmailOutbox>.Update.Set("DateTimeUtcProcessed", DateTime.UtcNow);
                var updateResult = await _emailEntityCollection.UpdateOneAsync(updateFilter, update);
                var timeTaken = stopwatch.Elapsed;
                stopwatch.Stop();

                _logger.LogInformation("UpdateResult: {result} records modified in {time}ms",
                  updateResult.ModifiedCount,
                  timeTaken.TotalMilliseconds);
            }
            catch (Exception ex)
            {
                // TODO: Log more details
                _logger.LogError("Failed to send email: {message}", ex.Message);
            }
        }
        else
        {
            _logger.LogInformation("No emails to send; sleeping...");
        }
    }
}
