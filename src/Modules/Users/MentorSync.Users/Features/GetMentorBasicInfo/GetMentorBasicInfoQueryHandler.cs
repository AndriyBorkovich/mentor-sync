using Ardalis.Result;
using MediatR;
using MentorSync.Users.Data;
using MentorSync.Users.Extensions;
using MentorSync.SharedKernel.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MentorSync.Users.Features.GetMentorBasicInfo;

internal sealed record MentorRatingsData(double Average, int Count);

public class GetMentorBasicInfoQueryHandler(UsersDbContext dbContext, ILogger<GetMentorBasicInfoQueryHandler> logger) : IRequestHandler<GetMentorBasicInfoQuery, Result<MentorBasicInfoResponse>>
{
    private readonly UsersDbContext _dbContext = dbContext;
    private readonly ILogger<GetMentorBasicInfoQueryHandler> _logger = logger;

    public async Task<Result<MentorBasicInfoResponse>> Handle(GetMentorBasicInfoQuery request, CancellationToken cancellationToken)
    {
        try
        {
            // Join User and MentorProfile to get basic mentor information
            var mentorInfo = await _dbContext.Users
                .Where(u => u.Id == request.MentorId && u.IsActive)
                .Join(
                    _dbContext.MentorProfiles,
                    user => user.Id,
                    profile => profile.MentorId,
                    (user, profile) => new
                    {
                        user.Id,
                        Name = user.UserName,
                        Title = profile.Position,
                        ProfileImage = user.ProfileImageUrl,
                        YearsOfExperience = profile.ExperienceYears,
                        profile.Bio,
                        profile.Availability,
                        profile.Skills,
                        profile.Industries,
                        profile.ProgrammingLanguages
                    }
                )
                .FirstOrDefaultAsync(cancellationToken);

            if (mentorInfo == null)
            {
                return Result.NotFound($"Mentor with ID {request.MentorId} not found.");
            }

            var ratings = await _dbContext.Database.SqlQuery<MentorRatingsData>// Assuming MentorReviews are in a different context
                ($@"
                    SELECT 
                        COALESCE(AVG(""Rating""), 0) AS ""Average"", 
                        COUNT(*) AS ""Count""
                    FROM ratings.""MentorReviews""
                    WHERE ""MentorId"" = {request.MentorId}"
                ).FirstOrDefaultAsync(cancellationToken);


            var response = new MentorBasicInfoResponse
            {
                Id = mentorInfo.Id,
                Name = mentorInfo.Name,
                Title = mentorInfo.Title,
                Rating = ratings?.Average ?? 0.0,
                ReviewsCount = ratings?.Count ?? 0,
                ProfileImage = mentorInfo.ProfileImage,
                YearsOfExperience = mentorInfo.YearsOfExperience,
                Category = mentorInfo.Industries.GetCategories(),
                Bio = mentorInfo.Bio,
                Availability = mentorInfo.Availability.ToReadableString(),
                ProgrammingLanguages = mentorInfo.ProgrammingLanguages ?? [],
                Skills = [.. mentorInfo.Skills
                    .Select((skill, index) => new MentorSkillDto
                    {
                        Id = index.ToString(),
                        Name = skill
                    })]
            };

            return Result.Success(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting mentor basic info for MentorId: {MentorId}", request.MentorId);
            return Result.Error($"An error occurred while getting mentor basic info: {ex.Message}");
        }
    }
}
