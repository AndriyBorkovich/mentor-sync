using MediatR;
using Ardalis.Result;

namespace MentorSync.Notifications.Contracts;

public sealed class SendEmailCommand : IRequest<Result<string>>
{
    public string To { get; set; } = string.Empty;
    public string From { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
}
