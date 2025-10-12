namespace MentorSync.Notifications.Features.GetAllMessages;

/// <summary>
/// Represents the response containing details of a notification message.
/// </summary>
/// <param name="Id">The unique identifier of the message.</param>
/// <param name="To">The recipient of the message.</param>
/// <param name="From">The sender of the message.</param>
/// <param name="Subject">The subject line of the message.</param>
/// <param name="DateTimeUtcProcessed">The UTC date and time when the message was processed, or <c>null</c> if not processed.</param>
/// <example>
/// <code>
/// var response = new GetAllMessagesResponse(
///     Id: "msg-123",
///     To: "user@example.com",
///     From: "admin@mentorsync.com",
///     Subject: "Welcome to MentorSync",
///     DateTimeUtcProcessed: DateTime.UtcNow
/// );
/// </code>
/// </example>
public sealed record GetAllMessagesResponse(
	string Id,
	string To,
	string From,
	string Subject,
	DateTime? DateTimeUtcProcessed);
