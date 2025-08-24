using MentorSync.SharedKernel;
using MentorSync.SharedKernel.Abstractions.Endpoints;
using MentorSync.SharedKernel.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace MentorSync.Users.Features.GetMentorBasicInfo;

public sealed class GetMentorBasicInfoEndpoint : IEndpoint
{
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
			.Produces<MentorBasicInfoResponse>(StatusCodes.Status200OK)
			.Produces(StatusCodes.Status404NotFound)
			.RequireAuthorization(PolicyConstants.ActiveUserOnly, PolicyConstants.AdminMenteeMix);
	}
}
