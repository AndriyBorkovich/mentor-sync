using MediatR;
using MentorSync.SharedKernel.CommonEntities;

namespace MentorSync.Users.Features.SearchMentors;

public sealed record SearchMentorsQuery(
    string SearchTerm = null,
    List<string> ProgrammingLanguages = null,
    Industry? Industry = null,
    int? MinExperienceYears = null,
    int PageNumber = 1,
    int PageSize = 10) : IRequest<PaginatedList<MentorSearchResponse>>;
