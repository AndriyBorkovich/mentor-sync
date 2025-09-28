using System.Security.Claims;
using MentorSync.SharedKernel;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace MentorSync.Users.Features.GetUserProfile;

public sealed class GetUserProfileEndpoint : IEndpoint
{
	public void MapEndpoint(IEndpointRouteBuilder app)
	{
		app.MapGet("/users/profile", async (
			HttpContext httpContext,
			IMediator mediator,
			CancellationToken ct) =>
		{
			var userIdClaim = httpContext.User.FindFirst(ClaimTypes.NameIdentifier);
			if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var userId))
			{
				return Results.Problem("User ID not found or invalid", statusCode: 401);
			}

			var result = await mediator.SendQueryAsync<GetUserProfileQuery, UserProfileResponse>(new GetUserProfileQuery(userId), ct);

			return result.DecideWhatToReturn();
		})
		.WithTags(TagsConstants.Users)
		.WithDescription("Get user profile information including related mentee/mentor profile")
		.RequireAuthorization(PolicyConstants.ActiveUserOnly)
		.Produces<UserProfileResponse>()
		.ProducesProblem(StatusCodes.Status404NotFound);
	}
}
