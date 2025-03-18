using Ardalis.Result;
using MediatR;
using MentorSync.Notifications.Data;
using MentorSync.Notifications.Domain;
using MentorSync.Notifications.MappingExtensions;
using MongoDB.Driver;

namespace MentorSync.Notifications.Features.GetAllMessages;

public class GetAllMessagesQueryHandler(MongoDbContext dbContext)
    : IRequestHandler<GetAllMessagesQuery, Result<List<GetAllMessagesResponse>>>
{
    public async Task<Result<List<GetAllMessagesResponse>>> Handle(
        GetAllMessagesQuery request,
        CancellationToken cancellationToken)
    {
        var filter = Builders<EmailOutbox>.Filter.Empty;
        var messages = await dbContext.EmailOutboxes.Find(filter).ToListAsync(cancellationToken: cancellationToken);

        return messages.Count == 0 ? Result.NoContent() : Result.Success(messages.Select(m => m.ToResponse()).ToList());
    }
}
