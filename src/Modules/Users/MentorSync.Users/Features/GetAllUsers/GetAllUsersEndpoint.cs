using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace MentorSync.Users.Features.GetAllUsers;

/// <summary>
/// Endpoint to get all users with optional filtering by role and active status
/// </summary>
public sealed class GetAllUsersEndpoint : IEndpoint
{
	/// <inheritdoc />
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
			.Produces<List<UserShortResponse>>()
			.RequireAuthorization(PolicyConstants.ActiveUserOnly, PolicyConstants.AdminOnly);
	}
}
