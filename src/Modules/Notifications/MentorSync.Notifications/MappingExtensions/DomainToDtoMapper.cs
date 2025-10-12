using MentorSync.Notifications.Domain;
using MentorSync.Notifications.Features.GetAllMessages;

namespace MentorSync.Notifications.MappingExtensions;

/// <summary>
/// Extension methods for mapping domain models to DTOs
/// </summary>
public static class DomainToDtoMapper
{
	/// <summary>
	/// Maps an EmailOutbox domain model to a GetAllMessagesResponse DTO
	/// </summary>
	/// <param name="emailOutbox">Outbox entity></param>
	/// <returns>Mapped response model</returns>
	public static GetAllMessagesResponse ToResponse(this EmailOutbox emailOutbox)
	{
		return new GetAllMessagesResponse(
			emailOutbox.Id.ToString(),
			emailOutbox.To,
			emailOutbox.From,
			emailOutbox.Subject,
			emailOutbox.DateTimeUtcProcessed);
	}
}
