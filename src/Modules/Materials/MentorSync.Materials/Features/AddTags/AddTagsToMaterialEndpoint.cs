using System.Security.Claims;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace MentorSync.Materials.Features.AddTags;

/// <summary>
/// Endpoint for adding tags to an existing learning material
/// </summary>
public sealed class AddTagsToMaterialEndpoint : IEndpoint
{
	/// <inheritdoc />
	public void MapEndpoint(IEndpointRouteBuilder app)
	{
		app.MapPost("materials/{materialId}/tags", async (
			int materialId,
			AddTagsRequest request,
			IMediator mediator,
			HttpContext httpContext,
			CancellationToken cancellationToken) =>
			{
				var userIdClaim = httpContext.User.FindFirst(ClaimTypes.NameIdentifier);
				if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var mentorId))
				{
					return Results.Problem("User ID not found or invalid", statusCode: StatusCodes.Status400BadRequest);
				}

				var command = new AddTagsToMaterialCommand
				{
					MaterialId = materialId,
					TagNames = request.TagNames,
					MentorId = mentorId
				};

				var result = await mediator.SendCommandAsync<AddTagsToMaterialCommand, AddTagsResponse>(command, cancellationToken);

				return result.DecideWhatToReturn();
			})
			.WithTags(TagsConstants.Materials)
			.WithDescription("Adds tags to an existing learning material")
			.Produces<AddTagsResponse>()
			.ProducesProblem(StatusCodes.Status400BadRequest)
			.ProducesProblem(StatusCodes.Status403Forbidden)
			.ProducesProblem(StatusCodes.Status404NotFound);
	}
}
