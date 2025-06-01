using Ardalis.Result;
using MediatR;
using MentorSync.SharedKernel.CommonEntities;

namespace MentorSync.Recommendations.Features.GetRecommendedMentors;

/// <summary>
/// Query to get recommended mentors for the currently logged-in mentee
/// </summary>
public sealed record GetRecommendedMentorsQuery(
    int MenteeId,
    string SearchTerm = null,
    List<string> ProgrammingLanguages = null,
    Industry? Industry = null,
    int? MinExperienceYears = null,
    int PageNumber = 1,
    int PageSize = 10) : IRequest<Result<PaginatedList<RecommendedMentorResponse>>>;
