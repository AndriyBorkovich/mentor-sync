using MentorSync.SharedKernel;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace MentorSync.Scheduling.Features.MentorAvailability.Delete;

public sealed class DeleteMentorAvailabilityEndpoint : IEndpoint
{
	public void MapEndpoint(IEndpointRouteBuilder app)
	{
		app.MapDelete("/scheduling/mentors/{mentorId:int}/availability/{availabilityId:int}", async (
			[FromRoute] int mentorId,
			[FromRoute] int availabilityId,
			IMediator mediator,
			CancellationToken cancellationToken) =>
		{
			var command = new DeleteMentorAvailabilityCommand(
				mentorId,
				availabilityId);

			var result = await mediator.SendCommandAsync<DeleteMentorAvailabilityCommand, string>(command, cancellationToken);

			return result.DecideWhatToReturn();
		})
		.WithTags(TagsConstants.Scheduling)
		.WithDescription("Deletes an availability slot for a mentor")
		.Produces<string>()
		.ProducesProblem(StatusCodes.Status404NotFound)
		.RequireAuthorization(PolicyConstants.ActiveUserOnly, PolicyConstants.MentorOnly);
	}
}
