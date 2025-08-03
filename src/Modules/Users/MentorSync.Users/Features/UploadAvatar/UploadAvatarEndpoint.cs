using MentorSync.SharedKernel;
using MentorSync.SharedKernel.Abstractions.Endpoints;
using MentorSync.SharedKernel.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace MentorSync.Users.Features.UploadAvatar;

public sealed class UploadAvatarEndpoint : IEndpoint
{
	public void MapEndpoint(IEndpointRouteBuilder app)
	{
		app.MapPost("/users/{id:int}/avatar", async (
			int id,
			IFormFile file,
			IMediator mediator) =>
		{
			var result = await mediator.SendCommandAsync<UploadAvatarCommand, string>(new UploadAvatarCommand(id, file));

			return result.DecideWhatToReturn();
		})
		.WithTags(TagsConstants.Users)
		.WithDescription("""
            Upload user profile image.
            Replaces existing image if it exists.
            """)
		.Accepts<IFormFile>("multipart/form-data")
		.Produces<string>(StatusCodes.Status200OK)
		.ProducesProblem(StatusCodes.Status404NotFound)
		.RequireAuthorization(PolicyConstants.ActiveUserOnly)
		.DisableAntiforgery();
	}
}
