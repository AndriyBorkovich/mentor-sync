using MediatR;
using MentorSync.Users.Data;
using Microsoft.EntityFrameworkCore;

namespace MentorSync.Users.Features.SearchMentors;

public sealed partial class SearchMentorsQueryHandler(
    UsersDbContext usersDbContext) : IRequestHandler<SearchMentorsQuery, List<MentorSearchResponse>>
{
    private static readonly FormattableString SqlQuery = $@"
        SELECT DISTINCT ON (u.""Id"")
            u.""Id"",
            u.""UserName"" as ""Name"",
            mp.""Position"" as ""Title"",
            COALESCE(AVG(mr.""Rating"") OVER (PARTITION BY mp.""MentorId""), 0) AS ""Rating"",
            mp.""Skills"",
            u.""ProfileImageUrl"" as ""ProfileImage"",
            mp.""ExperienceYears"",
            CASE 
                WHEN (mp.""Industries"" & 1) = 1 THEN 'Веб розробка'
                WHEN (mp.""Industries"" & 2) = 2 THEN 'Наука даних'
                WHEN (mp.""Industries"" & 4) = 4 THEN 'Кібербезпека'
                WHEN (mp.""Industries"" & 8) = 8 THEN 'Хмарні обчислення'
                WHEN (mp.""Industries"" & 16) = 16 THEN 'DevOps'
                WHEN (mp.""Industries"" & 32) = 32 THEN 'Розробка ігор'
                WHEN (mp.""Industries"" & 64) = 64 THEN 'ІТ-підтрика'
                WHEN (mp.""Industries"" & 128) = 128 THEN 'Штучний інтелект'
                WHEN (mp.""Industries"" & 256) = 256 THEN 'Блокчейн'
                WHEN (mp.""Industries"" & 512) = 512 THEN 'Мережі'
                WHEN (mp.""Industries"" & 1024) = 1024 THEN 'UX/UI дизайн'
                WHEN (mp.""Industries"" & 2048) = 2048 THEN 'Вбудовані системи'
                WHEN (mp.""Industries"" & 4096) = 4096 THEN 'IT консалтинг'
                WHEN (mp.""Industries"" & 8192) = 8192 THEN 'Адміністрування баз даних'
                WHEN (mp.""Industries"" & 16384) = 16384 THEN 'Управління проектами'
                WHEN (mp.""Industries"" & 32768) = 32768 THEN 'Мобільна розробка'
                WHEN (mp.""Industries"" & 65536) = 65536 THEN 'Low-code/No-code'
                WHEN (mp.""Industries"" & 131072) = 131072 THEN 'QA/QC'
                WHEN (mp.""Industries"" & 262144) = 262144 THEN 'Машинне навчання'
                ELSE 'Інше'
            END as ""Category"",
            mp.""ProgrammingLanguages"",
            mp.""Industries"",
            u.""IsActive""
        FROM users.""Users"" u
        INNER JOIN users.""MentorProfiles"" mp ON u.""Id"" = mp.""MentorId""
        LEFT JOIN ratings.""MentorReviews"" mr ON mp.""MentorId"" = mr.""MentorId""";

    public async Task<List<MentorSearchResponse>> Handle(SearchMentorsQuery request, CancellationToken cancellationToken)
    {
        var mentorsQuery = usersDbContext.Database
            .SqlQuery<MentorSearchResultDto>(SqlQuery);

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            var searchTerm = request.SearchTerm.ToLower();
            mentorsQuery = mentorsQuery.Where(m =>
                m.Name.ToLower().Contains(searchTerm) ||
                m.Title.ToLower().Contains(searchTerm) ||
                m.Skills.Any(s => s.ToLower().Contains(searchTerm)));
        }

        if (request.ProgrammingLanguages != null && request.ProgrammingLanguages.Count != 0)
        {
            mentorsQuery = mentorsQuery.Where(m =>
                m.ProgrammingLanguages != null &&
                request.ProgrammingLanguages.Any(lang =>
                    m.ProgrammingLanguages.Contains(lang)));
        }

        if (request.Industry.HasValue && request.Industry.Value != 0)
        {
            var industryValue = (int)request.Industry.Value;
            mentorsQuery = mentorsQuery.Where(m => (m.Industries & industryValue) > 0);
        }

        if (request.MinExperienceYears.HasValue)
        {
            mentorsQuery = mentorsQuery.Where(m =>
                m.ExperienceYears.HasValue &&
                m.ExperienceYears.Value >= request.MinExperienceYears.Value);
        }

        mentorsQuery = mentorsQuery.Where(m => m.IsActive);
        mentorsQuery = mentorsQuery.OrderByDescending(m => m.Rating);

        // Execute the query
        var mentors = await mentorsQuery.ToListAsync(cancellationToken);
        var response = mentors.Select(m => new MentorSearchResponse(
            m.Id,
            m.Name,
            m.Title,
            m.Rating,
            ConvertSkillsToList(m.Skills),
            m.ProfileImage,
            m.ExperienceYears,
            m.Category
        )).ToList();

        return response;
    }

    private static List<SkillResponse> ConvertSkillsToList(string[] skills)
    {
        // Convert skills array to list of SkillResponse objects
        return skills.Select((skill, index) => new SkillResponse(index.ToString(), skill)).ToList();
    }
}

