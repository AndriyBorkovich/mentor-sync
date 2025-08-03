using MentorSync.SharedKernel.CommonEntities;
using MentorSync.SharedKernel.CommonEntities.Enums;

namespace MentorSync.Recommendations.Features.GetRecommendedMentors;

/// <summary>
/// Query to get recommended mentors for the currently logged-in mentee
/// </summary>
public sealed record GetRecommendedMentorsQuery(
	int MenteeId,
	string SearchTerm = null,
	IReadOnlyList<string> ProgrammingLanguages = null,
	Industry? Industry = null,
	int? MinExperienceYears = null,
	double? MinRating = null,
	double? MaxRating = null,
	int PageNumber = 1,
	int PageSize = 10) : IQuery<PaginatedList<RecommendedMentorResponse>>;
