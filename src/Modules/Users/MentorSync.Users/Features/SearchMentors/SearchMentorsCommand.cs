using Ardalis.Result;
using MediatR;
using MentorSync.SharedKernel.CommonEntities;

namespace MentorSync.Users.Features.SearchMentors;

public sealed record SearchMentorsCommand(
    string SearchTerm = null,
    List<string> ProgrammingLanguages = null,
    Industry? Industry = null,
    int? MinExperienceYears = null) : IRequest<List<MentorSearchResponse>>;
