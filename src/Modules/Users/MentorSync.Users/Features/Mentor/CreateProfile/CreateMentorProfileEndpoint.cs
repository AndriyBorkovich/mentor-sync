using MentorSync.Users.Features.Common.Responses;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace MentorSync.Users.Features.Mentor.CreateProfile;

/// <summary>
/// Endpoint to create a new mentor profile
/// </summary>
public sealed class CreateMentorProfileEndpoint : IEndpoint
{
	/// <inheritdoc />
	public void MapEndpoint(IEndpointRouteBuilder app)
	{
		app.MapPost("/mentors/profile", async (
			[FromBody] CreateMentorProfileCommand request,
			IMediator mediator,
			CancellationToken ct) =>
		{
			var result = await mediator.SendCommandAsync<CreateMentorProfileCommand, MentorProfileResponse>(request, ct);

			return result.DecideWhatToReturn();
		})
		.WithTags(TagsConstants.Mentors)
		.WithDescription("Create new mentor profile")
		.Produces<MentorProfileResponse>()
		.ProducesProblem(StatusCodes.Status409Conflict)
		.RequireAuthorization(PolicyConstants.ActiveUserOnly, PolicyConstants.AdminMentorMix);
	}
}
