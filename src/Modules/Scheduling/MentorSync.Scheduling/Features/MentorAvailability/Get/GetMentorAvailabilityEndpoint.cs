using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace MentorSync.Scheduling.Features.MentorAvailability.Get;

/// <summary>
/// Endpoint to get availability slots for a mentor within a date range
/// </summary>
public sealed class GetMentorAvailabilityEndpoint : IEndpoint
{
	/// <inheritdoc />
	public void MapEndpoint(IEndpointRouteBuilder app)
	{
		app.MapGet("/scheduling/mentors/{mentorId:int}/availability", async (
			[FromRoute] int mentorId,
			[FromQuery] DateTime startDate,
			[FromQuery] DateTime endDate,
			IMediator mediator,
			CancellationToken cancellationToken) =>
		{
			var start = startDate != default
				? new DateTimeOffset(startDate)
				: DateTimeOffset.UtcNow;

			var end = endDate != default
				? new DateTimeOffset(endDate).AddDays(1).AddSeconds(-1)
				: start.AddDays(7).AddSeconds(-1);

			var query = new GetMentorAvailabilityQuery(mentorId, start, end);
			var result = await mediator.SendQueryAsync<GetMentorAvailabilityQuery, MentorAvailabilityResult>(query, cancellationToken);

			return result.DecideWhatToReturn();
		})
		.WithTags(TagsConstants.Scheduling)
		.WithDescription("Gets availability slots for a mentor within a date range")
		.Produces<MentorAvailabilityResult>()
		.ProducesProblem(StatusCodes.Status404NotFound)
		.RequireAuthorization(PolicyConstants.ActiveUserOnly, PolicyConstants.MentorMenteeMix);
	}
}
