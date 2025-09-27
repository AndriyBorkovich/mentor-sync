using MentorSync.SharedKernel;
using MentorSync.Users.Features.Common.Responses;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace MentorSync.Users.Features.Mentor.EditProfile;

public sealed class EditMentorProfileEndpoint : IEndpoint
{
	public void MapEndpoint(IEndpointRouteBuilder app)
	{
		app.MapPut("/mentors/profile", async (
				[FromBody] EditMentorProfileCommand command,
				IMediator mediator,
				CancellationToken ct) =>
		{
			var result = await mediator.SendCommandAsync<EditMentorProfileCommand, MentorProfileResponse>(command, ct);
			return result.DecideWhatToReturn();
		})
		.WithTags(TagsConstants.Mentors)
		.WithDescription("Edit mentor profile")
		.Produces<MentorProfileResponse>()
		.ProducesProblem(StatusCodes.Status400BadRequest)
		.RequireAuthorization(PolicyConstants.ActiveUserOnly, PolicyConstants.AdminMentorMix);
	}
}
