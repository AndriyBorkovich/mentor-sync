using System.ComponentModel.DataAnnotations;
using MentorSync.Users.Domain.User;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace MentorSync.Users.Features.ResendConfirmation;

/// <summary>
/// Endpoint to resend confirmation email to user
/// </summary>
public sealed class ResendConfirmationEndpoint : IEndpoint
{
	/// <inheritdoc />
	public void MapEndpoint(IEndpointRouteBuilder app)
	{
		app.MapGet("/users/reconfirm", async (
			[FromQuery, Required] int userId,
			IPublisher<UserCreatedEvent> eventPublisher,
			CancellationToken cancellationToken) =>
		{
			await eventPublisher.PublishAsync(new UserCreatedEvent(userId), cancellationToken);

			return Results.Ok("Confirmation email sent");
		})
		.WithTags(TagsConstants.Users)
		.WithDescription("Resend confirmation email to user")
		.Produces<string>()
		.Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
		.RequireAuthorization(PolicyConstants.AdminOnly);
	}
}
