using System.Security.Claims;
using MentorSync.SharedKernel;
using MentorSync.SharedKernel.Abstractions.Endpoints;
using MentorSync.SharedKernel.CommonEntities;
using MentorSync.SharedKernel.CommonEntities.Enums;
using MentorSync.SharedKernel.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace MentorSync.Recommendations.Features.GetRecommendedMaterials;

public sealed class GetRecommendedMaterialsEndpoint : IEndpoint
{
	public void MapEndpoint(IEndpointRouteBuilder app)
	{
		app.MapGet("/recommendations/materials", async (
			[FromQuery] string searchTerm,
			[FromQuery] string[] tags,
			[FromQuery] MaterialType? type,
			[FromQuery] int? pageNumber,
			[FromQuery] int? pageSize,
			IMediator sender,
			HttpContext httpContext,
			CancellationToken cancellationToken) =>
		{
			var userIdClaim = httpContext.User.FindFirst(ClaimTypes.NameIdentifier);
			if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var userId))
			{
				return Results.Problem("User ID not found or invalid", statusCode: 401);
			}

			var result = await sender.SendQueryAsync<GetRecommendedMaterialsQuery, PaginatedList<RecommendedMaterialResponse>>
				(new(
					userId,
					searchTerm,
					tags?.ToList() ?? [],
					type,
					pageNumber ?? 1,
					pageSize ?? 10), cancellationToken);

			return result.DecideWhatToReturn();
		})
		.WithTags(TagsConstants.Recommendations)
		.WithDescription("Get recommended learning materials for the currently logged-in mentee with filtering options")
		.Produces<PaginatedList<RecommendedMaterialResponse>>(StatusCodes.Status200OK)
		.ProducesProblem(StatusCodes.Status404NotFound)
		.RequireAuthorization(PolicyConstants.ActiveUserOnly, PolicyConstants.MenteeOnly);
	}
}
