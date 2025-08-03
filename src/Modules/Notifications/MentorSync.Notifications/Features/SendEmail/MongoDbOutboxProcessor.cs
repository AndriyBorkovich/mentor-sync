using System.Diagnostics;
using MentorSync.Notifications.Data;
using MentorSync.Notifications.Domain;
using MentorSync.Notifications.Infrastructure.Emails;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace MentorSync.Notifications.Features.SendEmail;

public sealed class EmailOutboxProcessor(
	IEmailSender emailSender,
	IServiceProvider serviceProvider,
	ILogger<EmailOutboxProcessor> logger)
	: IOutboxProcessor
{
	/// <summary>
	/// Checks for any emails that haven't been sent yet and sends them
	/// </summary>
	/// <returns></returns>
	public async Task CheckForEmailsToSend()
	{
		using var scope = serviceProvider.CreateScope();
		var dbContext = scope.ServiceProvider.GetRequiredService<NotificationsDbContext>();
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
				logger.LogInformation("Sending email {Id}", unsentEmailEntity.Id);

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

				logger.LogInformation("Email {Id} processed. {Result} record(s) updated in {Time}ms",
					unsentEmailEntity.Id,
					updateResult.ModifiedCount,
					stopwatch.Elapsed.TotalMilliseconds);
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "Failed to send email {Id}: {Message}", unsentEmailEntity.Id, ex.Message);
			}
		}
	}
}
