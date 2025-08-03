using MentorSync.SharedKernel;
using MentorSync.SharedKernel.Abstractions.Endpoints;
using MentorSync.SharedKernel.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace MentorSync.Scheduling.Features.MentorAvailability.Create;

public sealed class CreateMentorAvailabilityEndpoint : IEndpoint
{
	public void MapEndpoint(IEndpointRouteBuilder app)
	{
		app.MapPost("/scheduling/mentors/{mentorId:int}/availability", async (
			[FromRoute] int mentorId,
			[FromBody] CreateMentorAvailabilityRequest request,
			IMediator mediator) =>
		{
			var command = new CreateMentorAvailabilityCommand(
				mentorId,
				request.Start,
				request.End);

			var result = await mediator
							   .SendCommandAsync<CreateMentorAvailabilityCommand, CreateMentorAvailabilityResult>(command);

			return result.DecideWhatToReturn();
		})
		.WithTags(TagsConstants.Scheduling)
		.WithDescription("Creates a new availability slot for a mentor")
		.Produces<CreateMentorAvailabilityResult>(StatusCodes.Status200OK)
		.Produces(StatusCodes.Status400BadRequest)
		.Produces(StatusCodes.Status403Forbidden)
		.RequireAuthorization(PolicyConstants.ActiveUserOnly, PolicyConstants.MentorOnly);
	}
}
