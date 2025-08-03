using System.Security.Claims;
using MentorSync.SharedKernel;
using MentorSync.SharedKernel.Abstractions.Endpoints;
using MentorSync.SharedKernel.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace MentorSync.Materials.Features.UploadMaterialAttachment;

public sealed class UploadMaterialAttachmentEndpoint : IEndpoint
{
	public void MapEndpoint(IEndpointRouteBuilder app)
	{
		app.MapPost("materials/{materialId}/attachments", async (
			int materialId,
			IFormFile file,
			IMediator mediator,
			HttpContext httpContext) =>
			{
				var userIdClaim = httpContext.User.FindFirst(ClaimTypes.NameIdentifier);
				if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var mentorId))
				{
					return Results.Problem("User ID not found or invalid", statusCode: StatusCodes.Status400BadRequest);
				}

				var command = new UploadMaterialAttachmentCommand
				{
					MaterialId = materialId,
					File = file,
					MentorId = mentorId
				};

				var result = await mediator.SendCommandAsync<UploadMaterialAttachmentCommand, UploadAttachmentResponse>(command);

				return result.DecideWhatToReturn();
			})
			.WithTags(TagsConstants.Materials)
			.WithDescription("Uploads an attachment for a learning material")
			.Accepts<IFormFile>("multipart/form-data")
			.Produces<UploadAttachmentResponse>(StatusCodes.Status201Created)
			.ProducesProblem(StatusCodes.Status400BadRequest)
			.ProducesProblem(StatusCodes.Status403Forbidden)
			.ProducesProblem(StatusCodes.Status404NotFound)
			.RequireAuthorization(PolicyConstants.ActiveUserOnly, PolicyConstants.MentorOnly)
			.DisableAntiforgery(); // For file uploads
	}
}
