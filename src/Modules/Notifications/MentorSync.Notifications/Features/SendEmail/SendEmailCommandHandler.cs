﻿using Ardalis.Result;
using MediatR;
using MentorSync.Notifications.Contracts;
using MentorSync.Notifications.Data;
using MentorSync.Notifications.Domain;
using MongoDB.Bson;

namespace MentorSync.Notifications.Features.SendEmail;

public sealed class SendEmailCommandHandler(NotificationsDbContext dbContext) : IRequestHandler<SendEmailCommand, Result<string>>
{
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

        await dbContext.EmailOutboxes.InsertOneAsync(emailEntity, cancellationToken: ct);

        return id.ToString();
    }
}
