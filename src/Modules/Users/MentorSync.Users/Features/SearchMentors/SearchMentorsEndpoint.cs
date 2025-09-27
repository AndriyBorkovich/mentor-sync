using MentorSync.SharedKernel;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace MentorSync.Users.Features.SearchMentors;

public sealed class SearchMentorsEndpoint : IEndpoint
{
	public void MapEndpoint(IEndpointRouteBuilder app)
	{
		app.MapGet("users/mentors/search", async (
			[FromQuery] string searchTerm,
			[FromQuery] string[] programmingLanguages,
			[FromQuery] int? industry,
			[FromQuery] int? minExperienceYears,
			[FromQuery] double? minRating,
			[FromQuery] double? maxRating,
			[FromQuery] int pageNumber,
			[FromQuery] int pageSize,
			IMediator mediator,
			CancellationToken cancellationToken) =>
			{
				var industryEnum = industry.HasValue ? (Industry?)industry.Value : null;
				var programmingLanguagesList = programmingLanguages?.ToList();

				var result = await mediator.SendQueryAsync<SearchMentorsQuery, PaginatedList<MentorSearchResponse>>(new SearchMentorsQuery(
					searchTerm,
					programmingLanguagesList,
					industryEnum,
					minExperienceYears,
					minRating,
					maxRating,
					pageNumber,
					pageSize), cancellationToken);

				return result.DecideWhatToReturn();
			})
			.WithTags(TagsConstants.Users)
			.WithDescription("Search mentors with filters")
			.Produces<List<MentorSearchResponse>>()
			.RequireAuthorization(PolicyConstants.AdminMenteeMix);
	}
}
