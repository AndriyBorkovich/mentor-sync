using Ardalis.Result;
using MediatR;
using MentorSync.Recommendations.Data;
using MentorSync.SharedKernel.CommonEntities;
using MentorSync.SharedKernel.Extensions;
using Microsoft.EntityFrameworkCore;

namespace MentorSync.Recommendations.Features.GetRecommendedMentors;

public sealed class GetRecommendedMentorsQueryHandler(
    RecommendationsDbContext recommendationsContext)
        : IRequestHandler<GetRecommendedMentorsQuery, Result<PaginatedList<RecommendedMentorResponse>>>
{
    public async Task<Result<PaginatedList<RecommendedMentorResponse>>> Handle(GetRecommendedMentorsQuery request, CancellationToken cancellationToken)
    {
        var menteeId = request.MenteeId;
        var mentorsQuery = recommendationsContext.Database
            .SqlQuery<RecommendedMentorResultDto>($@"
                SELECT DISTINCT ON (u.""Id"")
                    u.""Id"",
                    u.""UserName"" as ""Name"",
                    mp.""Position"" as ""Title"",
                    COALESCE(AVG(mr.""Rating"") OVER (PARTITION BY mp.""MentorId""), 0) AS ""Rating"",
                    mp.""Skills"",
                    u.""ProfileImageUrl"" as ""ProfileImage"",
                    mp.""ExperienceYears"",
                    mp.""ProgrammingLanguages"",
                    mp.""Industries"",
                    u.""IsActive"",
                    rr.""CollaborativeScore"",
                    rr.""ContentBasedScore"",
                    rr.""FinalScore""
                FROM users.""Users"" u
                INNER JOIN users.""MentorProfiles"" mp ON u.""Id"" = mp.""MentorId""
                LEFT JOIN ratings.""MentorReviews"" mr ON mp.""MentorId"" = mr.""MentorId""
                INNER JOIN recommendations.""MentorRecommendationResults"" rr ON mp.""MentorId"" = rr.""MentorId"" AND rr.""MenteeId"" = {menteeId}
                WHERE rr.""MenteeId"" = {menteeId} AND rr.""FinalScore"" != 'NaN' AND rr.""ContentBasedScore"" > 0.0 AND rr.""CollaborativeScore"" > 0.0");

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

        var searchedIndustry = request.Industry;
        if (searchedIndustry.HasValue && searchedIndustry.Value != 0)
        {
            mentorsQuery = mentorsQuery.Where(m => (m.Industries & searchedIndustry.Value) > 0);
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
        mentorsQuery = mentorsQuery.OrderByDescending(m => m.FinalScore);

        // Get total count before pagination
        var totalCount = await mentorsQuery.CountAsync(cancellationToken);

        if (totalCount == 0)
        {
            return Result.NotFound("No recommended mentors found");
        }

        // Apply pagination
        mentorsQuery = mentorsQuery
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize);

        var mentors = await mentorsQuery.ToListAsync(cancellationToken);

        if (mentors == null || mentors.Count == 0)
        {
            return Result.NotFound("No recommended mentors found on this page");
        }

        var items = mentors.Select(m => new RecommendedMentorResponse(
            m.Id,
            m.Name,
            m.Title,
            m.Rating,
            ConvertSkillsToList(m.Skills),
            m.ProfileImage,
            m.ExperienceYears,
            m.Industries.GetCategories(),
            float.IsNaN(m.CollaborativeScore) ? 0 : m.CollaborativeScore,
            float.IsNaN(m.ContentBasedScore) ? 0 : m.ContentBasedScore,
            float.IsNaN(m.FinalScore) ? 0 : m.FinalScore
        )).ToList();

        var paginatedList = new PaginatedList<RecommendedMentorResponse>
        {
            Items = items,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize,
            TotalCount = totalCount
        };

        return Result.Success(paginatedList);
    }

    private static List<RecommendedSkillResponse> ConvertSkillsToList(string[] skills)
    {
        if (skills == null)
            return [];

        // Convert skills array to list of SkillResponse objects
        return [.. skills.Select((skill, index) => new RecommendedSkillResponse(index.ToString(), skill))];
    }
}
