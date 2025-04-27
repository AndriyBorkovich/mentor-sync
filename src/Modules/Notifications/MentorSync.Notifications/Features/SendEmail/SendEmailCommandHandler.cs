using Ardalis.Result;
using MediatR;
using MentorSync.Notifications.Contracts;
using MentorSync.Notifications.Data;
using MentorSync.Notifications.Domain;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace MentorSync.Notifications.Features.SendEmail;

public sealed class SendEmailCommandHandler : IRequestHandler<SendEmailCommand, Result<string>>
{
    private readonly IMongoCollection<EmailOutbox> _emailEntityCollection;

    public SendEmailCommandHandler(IMongoClient mongoClient, IOptionsMonitor<MongoSettings> settings)
    {
        var database = mongoClient.GetDatabase(settings.CurrentValue.Database);
        _emailEntityCollection = database.GetCollection<EmailOutbox>(settings.CurrentValue.Collection);
    }

    public async Task<Result<string>> Handle(SendEmailCommand request, CancellationToken ct)
    {
        var id = ObjectId.GenerateNewId();

        var emailEntity = new EmailOutbox
        {
            Id = id,
            To = request.To,
            From = request.From,
            Subject = request.Subject,
            Body = request.Body
        };

        await _emailEntityCollection.InsertOneAsync(emailEntity, cancellationToken: ct);

        return id.ToString();
    }
}
