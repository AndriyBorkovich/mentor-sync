using Ardalis.Result;
using MentorSync.Users.Data;
using MentorSync.SharedKernel.Extensions;
using MentorSync.SharedKernel.CommonEntities;
using Microsoft.EntityFrameworkCore;
using MentorSync.SharedKernel.CommonEntities.Enums;

namespace MentorSync.Users.Features.SearchMentors;

public sealed class SearchMentorsQueryHandler(
	UsersDbContext usersDbContext)
	: IQueryHandler<SearchMentorsQuery, PaginatedList<MentorSearchResponse>>
{
	public async Task<Result<PaginatedList<MentorSearchResponse>>> Handle(SearchMentorsQuery request, CancellationToken cancellationToken = default)
	{
		var mentorsQuery = usersDbContext.Database
			.SqlQuery<MentorSearchResultDto>($@"
                SELECT DISTINCT ON (u.""Id"")
                    u.""Id"",
                    u.""UserName"" as ""Name"",
                    mp.""Position"" as ""Title"",
                    COALESCE(AVG(mr.""Rating"") OVER (PARTITION BY mp.""MentorId""), 0) AS ""Rating"",
                    mp.""Skills"",
                    u.""ProfileImageUrl"" as ""ProfileImage"",
                    mp.""ExperienceYears"",
                    mp.""Industries"",
                    mp.""ProgrammingLanguages"",
                    u.""IsActive""
                FROM users.""Users"" u
                INNER JOIN users.""MentorProfiles"" mp ON u.""Id"" = mp.""MentorId""
                LEFT JOIN ratings.""MentorReviews"" mr ON mp.""MentorId"" = mr.""MentorId""");

		if (!string.IsNullOrWhiteSpace(request.SearchTerm))
		{
			var pattern = $"%{request.SearchTerm.ToLower()}%";
			mentorsQuery = mentorsQuery
				.Where(m => EF.Functions.ILike(m.Name, pattern) ||
							EF.Functions.ILike(m.Title, pattern) ||
							m.Skills.Any(skill => EF.Functions.ILike(skill, pattern)));
		}

		if (request.ProgrammingLanguages != null && request.ProgrammingLanguages.Count != 0)
		{
			mentorsQuery = mentorsQuery.Where(m =>
				m.ProgrammingLanguages != null &&
				request.ProgrammingLanguages.Any(lang =>
					m.ProgrammingLanguages.Contains(lang)));
		}

		var searchedIndustry = request.Industry;
		if (searchedIndustry.HasValue && searchedIndustry.Value != Industry.None)
		{
			mentorsQuery = mentorsQuery.Where(m => (m.Industries & searchedIndustry.Value) == searchedIndustry.Value);
		}

		if (request.MinExperienceYears.HasValue)
		{
			mentorsQuery = mentorsQuery.Where(m =>
				m.ExperienceYears.HasValue &&
				m.ExperienceYears.Value >= request.MinExperienceYears.Value);
		}

		if (request.MinRating.HasValue)
		{
			mentorsQuery = mentorsQuery.Where(m => m.Rating >= request.MinRating.Value);
		}

		if (request.MaxRating.HasValue)
		{
			mentorsQuery = mentorsQuery.Where(m => m.Rating <= request.MaxRating.Value);
		}

		mentorsQuery = mentorsQuery.Where(m => m.IsActive);
		mentorsQuery = mentorsQuery.OrderByDescending(m => m.Rating);

		var totalCount = await mentorsQuery.CountAsync(cancellationToken);

		mentorsQuery = mentorsQuery
			.Skip((request.PageNumber - 1) * request.PageSize)
			.Take(request.PageSize);

		var mentors = await mentorsQuery.ToListAsync(cancellationToken);
		var items = mentors.ConvertAll(m => new MentorSearchResponse(
			m.Id,
			m.Name,
			m.Title,
			m.Rating,
			ConvertSkillsToList(m.Skills),
			m.ProfileImage,
			m.ExperienceYears,
			m.Industries.GetCategories()
		));

		var result = new PaginatedList<MentorSearchResponse>
		{
			Items = items,
			PageNumber = request.PageNumber,
			PageSize = request.PageSize,
			TotalCount = totalCount
		};

		return Result.Success(result);
	}

	private static List<SkillResponse> ConvertSkillsToList(string[] skills)
	{
		return [.. skills.Select((skill, index) => new SkillResponse(index.ToString(), skill))];
	}
}