using MentorSync.Users.Features.Common.Responses;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace MentorSync.Users.Features.Mentee.CreateProfile;

/// <summary>
/// Endpoint to create a new mentee profile
/// </summary>
public sealed class CreateMenteeProfileEndpoint : IEndpoint
{
	/// <inheritdoc />
	public void MapEndpoint(IEndpointRouteBuilder app)
	{
		app.MapPost("/mentees/profile", async (
			[FromBody] CreateMenteeProfileCommand request,
			IMediator mediator,
			CancellationToken ct) =>
		{
			var result = await mediator.SendCommandAsync<CreateMenteeProfileCommand, MenteeProfileResponse>(request, ct);

			return result.DecideWhatToReturn();
		})
		.WithTags(TagsConstants.Mentees)
		.WithDescription("Create new mentee profile")
		.Produces<MenteeProfileResponse>()
		.ProducesProblem(StatusCodes.Status409Conflict)
		.RequireAuthorization(PolicyConstants.ActiveUserOnly, PolicyConstants.AdminMenteeMix);
	}
}
