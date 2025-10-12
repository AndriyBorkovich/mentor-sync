using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace MentorSync.Users.Features.GetMentorBasicInfo;

/// <summary>
/// Endpoint to get basic profile information for a mentor
/// </summary>
public sealed class GetMentorBasicInfoEndpoint : IEndpoint
{
	/// <inheritdoc />
	public void MapEndpoint(IEndpointRouteBuilder app)
	{
		app.MapGet("users/mentors/{id:int}/basic-info", async (
			int id,
			IMediator mediator,
			CancellationToken cancellationToken) =>
			{
				var result = await mediator.SendQueryAsync<GetMentorBasicInfoQuery, MentorBasicInfoResponse>(new GetMentorBasicInfoQuery(id), cancellationToken);

				return result.DecideWhatToReturn();
			})
			.WithTags(TagsConstants.Mentors)
			.WithDescription("Gets basic profile information for a mentor")
			.Produces<MentorBasicInfoResponse>()
			.Produces(StatusCodes.Status404NotFound)
			.RequireAuthorization(PolicyConstants.ActiveUserOnly, PolicyConstants.AdminMenteeMix);
	}
}
