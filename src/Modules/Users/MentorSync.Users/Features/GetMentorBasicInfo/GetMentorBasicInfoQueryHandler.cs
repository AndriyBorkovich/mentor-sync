using System.Globalization;
using Ardalis.Result;
using MentorSync.Users.Data;
using MentorSync.Users.Extensions;
using Microsoft.EntityFrameworkCore;

namespace MentorSync.Users.Features.GetMentorBasicInfo;

/// <summary>
/// Helper record to hold mentor ratings data
/// </summary>
internal sealed record MentorRatingsData(double Average, int Count);

/// <summary>
/// Query handler to get basic mentor information
/// </summary>
/// <param name="dbContext">Database context</param>
public sealed class GetMentorBasicInfoQueryHandler(UsersDbContext dbContext) : IQueryHandler<GetMentorBasicInfoQuery, MentorBasicInfoResponse>
{
	/// <inheritdoc />
	public async Task<Result<MentorBasicInfoResponse>> Handle(GetMentorBasicInfoQuery request, CancellationToken cancellationToken = default)
	{
		// Join User and MentorProfile to get basic mentor information
		var mentorInfo = await dbContext.Users
			.Where(u => u.Id == request.MentorId && u.IsActive)
			.Join(
				dbContext.MentorProfiles,
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

		var ratings = await dbContext.Database.SqlQuery<MentorRatingsData>// Assuming MentorReviews are in a different context
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
			ProgrammingLanguages = mentorInfo.ProgrammingLanguages?.ToList() ?? [],
			Skills = [.. mentorInfo.Skills
				.Select((skill, index) => new MentorSkillDto
				{
					Id = index.ToString(CultureInfo.InvariantCulture),
					Name = skill
				})]
		};

		return Result.Success(response);
	}
}
