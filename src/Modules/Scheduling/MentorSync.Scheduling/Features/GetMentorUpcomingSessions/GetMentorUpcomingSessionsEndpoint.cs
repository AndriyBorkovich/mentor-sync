using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace MentorSync.Scheduling.Features.GetMentorUpcomingSessions;

/// <summary>
/// Endpoint to get upcoming sessions for a mentor
/// </summary>
public sealed class GetMentorUpcomingSessionsEndpoint : IEndpoint
{
	/// <inheritdoc />
	public void MapEndpoint(IEndpointRouteBuilder app)
	{
		app.MapGet("scheduling/mentors/{id:int}/upcoming-sessions", async (
			int id,
			IMediator mediator,
			CancellationToken cancellationToken) =>
			{
				var result = await mediator.SendQueryAsync<GetMentorUpcomingSessionsQuery, MentorUpcomingSessionsResponse>(new(id), cancellationToken);

				return result.DecideWhatToReturn();
			})
			.WithTags("Scheduling")
			.WithDescription("Gets upcoming sessions for a mentor")
			.Produces<MentorUpcomingSessionsResponse>()
			.Produces(StatusCodes.Status404NotFound)
			.AllowAnonymous();
	}
}
