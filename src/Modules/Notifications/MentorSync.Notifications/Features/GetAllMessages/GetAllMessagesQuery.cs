using Ardalis.Result;
using MediatR;

namespace MentorSync.Notifications.Features.GetAllMessages;

public record GetAllMessagesQuery : IRequest<Result<List<GetAllMessagesResponse>>>;
