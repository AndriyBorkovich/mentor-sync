using Ardalis.Result;
using MentorSync.Notifications.Data;
using MentorSync.Notifications.Domain;
using MentorSync.Notifications.MappingExtensions;
using MongoDB.Driver;

namespace MentorSync.Notifications.Features.GetAllMessages;

public sealed class GetAllMessagesQueryHandler(NotificationsDbContext dbContext)
	: IQueryHandler<GetAllMessagesQuery, List<GetAllMessagesResponse>>
{
	public async Task<Result<List<GetAllMessagesResponse>>> Handle(
		GetAllMessagesQuery request,
		CancellationToken cancellationToken)
	{
		var filter = Builders<EmailOutbox>.Filter.Empty;
		var messages = await dbContext.EmailOutboxes.Find(filter).ToListAsync(cancellationToken: cancellationToken);

		return messages.Count == 0 ? Result.NoContent() : Result.Success(messages.ConvertAll(m => m.ToResponse()));
	}
}
