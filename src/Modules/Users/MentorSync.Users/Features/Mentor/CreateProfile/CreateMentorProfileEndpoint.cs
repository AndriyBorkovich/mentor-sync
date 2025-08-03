using MentorSync.SharedKernel;
using MentorSync.SharedKernel.Abstractions.Endpoints;
using MentorSync.SharedKernel.Extensions;
using MentorSync.Users.Features.Common.Responses;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace MentorSync.Users.Features.Mentor.CreateProfile;

public sealed class CreateMentorProfileEndpoint : IEndpoint
{
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
		.Produces<MentorProfileResponse>(StatusCodes.Status200OK)
		.ProducesProblem(StatusCodes.Status409Conflict)
		.RequireAuthorization(PolicyConstants.ActiveUserOnly, PolicyConstants.AdminMentorMix);
	}
}
