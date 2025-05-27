using Ardalis.Result;
using MediatR;
using MentorSync.Users.Data;
using Microsoft.EntityFrameworkCore;

namespace MentorSync.Users.Features.SearchMentors;

public sealed class SearchMentorsCommandHandler(
    UsersDbContext usersDbContext) : IRequestHandler<SearchMentorsCommand, List<MentorSearchResponse>>
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
                WHEN (mp.""Industries"" & 1) = 1 THEN 'Web Development'
                WHEN (mp.""Industries"" & 2) = 2 THEN 'Data Science'
                WHEN (mp.""Industries"" & 4) = 4 THEN 'Cybersecurity'
                WHEN (mp.""Industries"" & 8) = 8 THEN 'Cloud Computing'
                WHEN (mp.""Industries"" & 16) = 16 THEN 'DevOps'
                WHEN (mp.""Industries"" & 32) = 32 THEN 'Game Development'
                WHEN (mp.""Industries"" & 64) = 64 THEN 'IT Support'
                WHEN (mp.""Industries"" & 128) = 128 THEN 'Artificial Intelligence'
                WHEN (mp.""Industries"" & 256) = 256 THEN 'Blockchain'
                WHEN (mp.""Industries"" & 512) = 512 THEN 'Networking'
                WHEN (mp.""Industries"" & 1024) = 1024 THEN 'UX/UI Design'
                WHEN (mp.""Industries"" & 2048) = 2048 THEN 'Embedded Systems'
                WHEN (mp.""Industries"" & 4096) = 4096 THEN 'IT Consulting'
                WHEN (mp.""Industries"" & 8192) = 8192 THEN 'Database Administration'
                ELSE 'Other'
            END as ""Category"",
            mp.""ProgrammingLanguages"",
            mp.""Industries"",
            u.""IsActive""
        FROM users.""Users"" u
        INNER JOIN users.""MentorProfiles"" mp ON u.""Id"" = mp.""MentorId""
        LEFT JOIN ratings.""MentorReviews"" mr ON mp.""MentorId"" = mr.""MentorId""";

    public async Task<List<MentorSearchResponse>> Handle(SearchMentorsCommand request, CancellationToken cancellationToken)
    {
        // Execute the query with LINQ for conditional filtering
        var mentorsQuery = usersDbContext.Database
            .SqlQuery<MentorSearchResultDto>(SqlQuery);

        // Apply filters using LINQ
        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            var searchTerm = request.SearchTerm.ToLower();
            mentorsQuery = mentorsQuery.Where(m =>
                m.Name.ToLower().Contains(searchTerm) ||
                m.Title.ToLower().Contains(searchTerm) ||
                m.Skills.Any(s => s.ToLower().Contains(searchTerm)));
        }

        // Filter by programming languages
        if (request.ProgrammingLanguages != null && request.ProgrammingLanguages.Count != 0)
        {
            mentorsQuery = mentorsQuery.Where(m =>
                m.ProgrammingLanguages != null &&
                request.ProgrammingLanguages.Any(lang =>
                    m.ProgrammingLanguages.Contains(lang)));
        }

        // Filter by industry
        if (request.Industry.HasValue && request.Industry.Value != 0)
        {
            var industryValue = (int)request.Industry.Value;
            mentorsQuery = mentorsQuery.Where(m => (m.Industries & industryValue) > 0);
        }

        // Filter by minimum experience years
        if (request.MinExperienceYears.HasValue)
        {
            mentorsQuery = mentorsQuery.Where(m =>
                m.ExperienceYears.HasValue &&
                m.ExperienceYears.Value >= request.MinExperienceYears.Value);
        }

        // Only active mentors
        mentorsQuery = mentorsQuery.Where(m => m.IsActive);        // Order by rating
        mentorsQuery = mentorsQuery.OrderByDescending(m => m.Rating);

        // Execute the query
        var mentors = await mentorsQuery.ToListAsync(cancellationToken);        // Map the results to the response model
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

    // DTO class for SQL query result
    public class MentorSearchResultDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public double Rating { get; set; }
        public string[] Skills { get; set; }
        public string ProfileImage { get; set; }
        public int? ExperienceYears { get; set; }
        public string Category { get; set; } // Matches the alias in the SQL query
        public string[] ProgrammingLanguages { get; set; }
        public int Industries { get; set; }
        public bool IsActive { get; set; }
    }
}

