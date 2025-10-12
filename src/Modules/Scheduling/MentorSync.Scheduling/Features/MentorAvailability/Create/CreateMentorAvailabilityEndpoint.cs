using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace MentorSync.Scheduling.Features.MentorAvailability.Create;

/// <summary>
/// Endpoint to create a new availability slot for a mentor
/// </summary>
public sealed class CreateMentorAvailabilityEndpoint : IEndpoint
{
	/// <inheritdoc />
	public void MapEndpoint(IEndpointRouteBuilder app)
	{
		app.MapPost("/scheduling/mentors/{mentorId:int}/availability", async (
			[FromRoute] int mentorId,
			[FromBody] CreateMentorAvailabilityRequest request,
			IMediator mediator,
			CancellationToken cancellationToken) =>
		{
			var command = new CreateMentorAvailabilityCommand(
				mentorId,
				request.Start,
				request.End);

			var result = await mediator
							   .SendCommandAsync<CreateMentorAvailabilityCommand, CreateMentorAvailabilityResult>(command, cancellationToken);

			return result.DecideWhatToReturn();
		})
		.WithTags(TagsConstants.Scheduling)
		.WithDescription("Creates a new availability slot for a mentor")
		.Produces<CreateMentorAvailabilityResult>()
		.ProducesProblem(StatusCodes.Status400BadRequest)
		.ProducesProblem(StatusCodes.Status403Forbidden)
		.RequireAuthorization(PolicyConstants.ActiveUserOnly, PolicyConstants.MentorOnly);
	}
}
