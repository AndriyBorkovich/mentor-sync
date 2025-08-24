using MentorSync.SharedKernel;
using MentorSync.SharedKernel.Extensions;
using MentorSync.SharedKernel.Abstractions.Endpoints;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace MentorSync.Users.Features.GetAllUsers;

public sealed class GetAllUsersEndpoint : IEndpoint
{
	public void MapEndpoint(IEndpointRouteBuilder app)
	{
		app.MapGet("users", async (
			[FromQuery] string role,
			[FromQuery] bool? isActive,
			IMediator mediator,
			CancellationToken cancellationToken)
				=>
			{
				var result = await mediator.SendQueryAsync<GetAllUsersQuery, List<UserShortResponse>>(new(role, isActive), cancellationToken);
				return result.DecideWhatToReturn();
			})
			.WithTags(TagsConstants.Users)
			.WithDescription("Get all users")
			.Produces<List<UserShortResponse>>(StatusCodes.Status200OK)
			.RequireAuthorization(PolicyConstants.ActiveUserOnly, PolicyConstants.AdminOnly);
	}
}
