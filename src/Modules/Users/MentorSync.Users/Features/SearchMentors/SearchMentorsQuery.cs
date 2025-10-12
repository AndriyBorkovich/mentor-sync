namespace MentorSync.Users.Features.SearchMentors;

/// <summary>
/// Query to search for mentors based on various criteria and paginate results
/// </summary>
public sealed record SearchMentorsQuery(
	string SearchTerm = null,
	IReadOnlyList<string> ProgrammingLanguages = null,
	Industry? Industry = null,
	int? MinExperienceYears = null,
	double? MinRating = null,
	double? MaxRating = null,
	int PageNumber = 1,
	int PageSize = 10) : IQuery<PaginatedList<MentorSearchResponse>>;
