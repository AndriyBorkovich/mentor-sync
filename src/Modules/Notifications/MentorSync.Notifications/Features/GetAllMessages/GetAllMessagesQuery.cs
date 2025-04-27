using Ardalis.Result;
using MediatR;

namespace MentorSync.Notifications.Features.GetAllMessages;

public sealed record GetAllMessagesQuery : IRequest<Result<List<GetAllMessagesResponse>>>;
