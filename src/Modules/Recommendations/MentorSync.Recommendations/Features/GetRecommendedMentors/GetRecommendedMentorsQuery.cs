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
    int MaxResults = 10) : IRequest<Result<List<RecommendedMentorResponse>>>;
