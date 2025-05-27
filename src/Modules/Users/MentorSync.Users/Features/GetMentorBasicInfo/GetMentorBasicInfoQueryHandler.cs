using Ardalis.Result;
using MediatR;
using MentorSync.Users.Data;
using MentorSync.Users.Domain.Enums;
using MentorSync.Users.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MentorSync.Users.Features.GetMentorBasicInfo;

internal sealed record AverateRating(double Value);

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
                        Id = user.Id,
                        Name = user.UserName,
                        Title = profile.Position,
                        ProfileImage = user.ProfileImageUrl,
                        YearsOfExperience = profile.ExperienceYears,
                        Bio = profile.Bio,
                        Availability = profile.Availability,
                        Skills = profile.Skills,
                        Industries = profile.Industries
                    }
                )
                .FirstOrDefaultAsync(cancellationToken);

            if (mentorInfo == null)
            {
                return Result.NotFound($"Mentor with ID {request.MentorId} not found.");
            }

            // Get average rating from MentorReviews
            var averageRating = await _dbContext.Database.SqlQuery<AverateRating>// Assuming MentorReviews are in a different context
                ($@"
                    SELECT COALESCE(AVG(""Rating""), 0) AS ""Value""
                    FROM ratings.""MentorReviews""
                    WHERE ""MentorId"" = {request.MentorId}"
                ).FirstOrDefaultAsync(cancellationToken);

            // Map industry enum to category string
            var category = GetCategoryFromIndustries((int)mentorInfo.Industries);

            // Create response
            var response = new MentorBasicInfoResponse
            {
                Id = mentorInfo.Id,
                Name = mentorInfo.Name,
                Title = mentorInfo.Title,
                Rating = averageRating?.Value ?? 0.0,
                ProfileImage = mentorInfo.ProfileImage,
                YearsOfExperience = mentorInfo.YearsOfExperience,
                Category = category,
                Bio = mentorInfo.Bio,
                Availability = GetReadableAvailability(mentorInfo.Availability),
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

    private static string GetCategoryFromIndustries(int industries)
    {
        // This matches the CASE expression in your original SQL query
        return industries switch
        {
            var i when (i & 1) == 1 => "Веб розробка",
            var i when (i & 2) == 2 => "Наука даних",
            var i when (i & 4) == 4 => "Кібербезпека",
            var i when (i & 8) == 8 => "Хмарні обчислення",
            var i when (i & 16) == 16 => "DevOps",
            var i when (i & 32) == 32 => "Розробка ігор",
            var i when (i & 64) == 64 => "IT Support",
            var i when (i & 128) == 128 => "Штучний інтелект",
            var i when (i & 256) == 256 => "Блокчейн",
            var i when (i & 512) == 512 => "Мережі",
            var i when (i & 1024) == 1024 => "UX/UI дизайн",
            var i when (i & 2048) == 2048 => "Вбудовані системи",
            var i when (i & 4096) == 4096 => "IT консалтинг",
            var i when (i & 8192) == 8192 => "Адміністрування баз даних",
            _ => "Other"
        };
    }

    private static string GetReadableAvailability(Availability availability)
    {
        return AvailabilityFormatter.ToReadableString(availability);
    }
}
