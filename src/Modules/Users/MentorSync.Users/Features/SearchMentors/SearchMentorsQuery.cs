using MediatR;
using MentorSync.SharedKernel.CommonEntities;
using MentorSync.SharedKernel.CommonEntities.Enums;

namespace MentorSync.Users.Features.SearchMentors;

public sealed record SearchMentorsQuery(
    string SearchTerm = null,
    List<string> ProgrammingLanguages = null,
    Industry? Industry = null,
    int? MinExperienceYears = null,
    double? MinRating = null,
    double? MaxRating = null,
    int PageNumber = 1,
    int PageSize = 10) : IRequest<PaginatedList<MentorSearchResponse>>;
