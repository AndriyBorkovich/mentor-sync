using MentorSync.SharedKernel;
using MentorSync.SharedKernel.Abstractions.Endpoints;
using MentorSync.SharedKernel.Extensions;
using MentorSync.Users.Features.Common.Responses;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace MentorSync.Users.Features.Mentee.EditProfile;

public sealed class EditMenteeProfileEndpoint : IEndpoint
{
	public void MapEndpoint(IEndpointRouteBuilder app)
	{
		app.MapPut("/mentees/profile", async (
				[FromBody] EditMenteeProfileCommand command,
				IMediator mediator,
				CancellationToken ct) =>
		{
			var result = await mediator.SendCommandAsync<EditMenteeProfileCommand, MenteeProfileResponse>(command, ct);
			return result.DecideWhatToReturn();
		})
		.WithTags(TagsConstants.Mentees)
		.WithDescription("Edit mentee profile")
		.Produces<MenteeProfileResponse>(StatusCodes.Status200OK)
		.ProducesProblem(StatusCodes.Status400BadRequest)
		.RequireAuthorization(PolicyConstants.ActiveUserOnly, PolicyConstants.AdminMenteeMix);
	}
}
