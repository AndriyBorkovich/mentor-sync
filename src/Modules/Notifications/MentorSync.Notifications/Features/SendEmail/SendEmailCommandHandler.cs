using Ardalis.Result;
using MentorSync.Notifications.Contracts;
using MentorSync.Notifications.Data;
using MentorSync.Notifications.Domain;
using MongoDB.Bson;

namespace MentorSync.Notifications.Features.SendEmail;

public sealed class SendEmailCommandHandler(NotificationsDbContext dbContext)
	: ICommandHandler<SendEmailCommand, string>
{
	public async Task<Result<string>> Handle(SendEmailCommand request, CancellationToken cancellationToken = default)
	{
		var id = ObjectId.GenerateNewId();

		var emailEntity = new EmailOutbox
		{
			Id = id,
			To = request.To,
			From = request.From,
			Subject = request.Subject,
			Body = request.Body,
		};

		await dbContext.EmailOutboxes.InsertOneAsync(emailEntity, cancellationToken: cancellationToken);

		return id.ToString();
	}
}
