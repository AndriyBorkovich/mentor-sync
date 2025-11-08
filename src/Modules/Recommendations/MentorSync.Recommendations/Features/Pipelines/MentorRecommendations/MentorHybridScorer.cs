using System.Globalization;
using MentorSync.Recommendations.Data;
using MentorSync.Recommendations.Domain.Result;
using MentorSync.Recommendations.Features.Pipelines.Base;
using MentorSync.Recommendations.Infrastructure.MachineLearning.Input;
using MentorSync.Recommendations.Infrastructure.MachineLearning.Output;
using MentorSync.Users.Contracts.Models;
using MentorSync.Users.Contracts.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.ML;

namespace MentorSync.Recommendations.Features.Pipelines.MentorRecommendations;

/// <inheritdoc />
public sealed class MentorHybridScorer(
	RecommendationsDbContext db,
	IMenteeProfileService menteeProfileService,
	IMentorProfileService mentorProfileService,
	ILogger<MentorHybridScorer> logger) : IHybridScorer
{
	private readonly MLContext _mlContext = new();

	/// <inheritdoc />
	public async Task GenerateRecommendationsAsync(CancellationToken cancellationToken)
	{
		var model = _mlContext.Model.Load(ServicesConstants.MentorModelFile, out _);
		logger.LogInformation("Generating hybrid recommendations for mentors...");

		var menteePreferences = await menteeProfileService.GetMenteesPreferences();
		var mentors = await mentorProfileService.GetAllMentorsAsync();

		var engine = _mlContext.Model.CreatePredictionEngine<MenteeMentorRatingData, MentorPrediction>(model);

		await ProcessPreferencesAsync(engine, menteePreferences, mentors.ToList(), cancellationToken);

		await db.SaveChangesAsync(cancellationToken);
		logger.LogInformation("Hybrid recommendations saved.");
	}

	private async Task ProcessPreferencesAsync(PredictionEngine<MenteeMentorRatingData, MentorPrediction> engine,
		IReadOnlyList<MenteePreferences> menteePreferences,
		IReadOnlyList<MentorProfileModel> mentors,
		CancellationToken cancellationToken)
	{
		foreach (var preferences in menteePreferences)
		{
			foreach (var mentor in mentors)
			{
				var cfScore = engine.Predict(new MenteeMentorRatingData
				{
					MenteeId = preferences.MenteeId.ToString(CultureInfo.InvariantCulture),
					MentorId = mentor.Id.ToString(CultureInfo.InvariantCulture)
				}).Score;

				var normalizedCfScore = float.IsNaN(cfScore) ? 0 : cfScore;
				var cbfScore = CalculateCbfScore(preferences, mentor);
				var finalScore = (normalizedCfScore * 0.7f) + (cbfScore * 0.3f);

				await UpsertResultAsync(preferences, mentor, normalizedCfScore, cbfScore, finalScore, cancellationToken);
			}
		}
	}

	private async Task UpsertResultAsync(MenteePreferences preferences,
		MentorProfileModel mentor,
		float normalizedCfScore,
		float cbfScore,
		float finalScore,
		CancellationToken cancellationToken)
	{
		var existingResult = await db.MentorRecommendationResults
			.FirstOrDefaultAsync(r =>
					r.MenteeId == preferences.MenteeId &&
					r.MentorId == mentor.Id,
				cancellationToken);

		if (existingResult is not null)
		{
			existingResult.CollaborativeScore = normalizedCfScore;
			existingResult.ContentBasedScore = cbfScore;
			existingResult.FinalScore = finalScore;
			existingResult.GeneratedAt = DateTime.UtcNow;
			db.MentorRecommendationResults.Update(existingResult);
		}
		else
		{
			db.MentorRecommendationResults.Add(new MentorRecommendationResult
			{
				MenteeId = preferences.MenteeId,
				MentorId = mentor.Id,
				CollaborativeScore = normalizedCfScore,
				ContentBasedScore = cbfScore,
				FinalScore = finalScore,
				GeneratedAt = DateTime.UtcNow
			});
		}
	}

	private static float CalculateCbfScore(MenteePreferences pref, MentorProfileModel mentor)
	{
		if (mentor is null)
		{
			return 0;
		}

		float score = 0;
		var preferredLanguages = pref?.DesiredProgrammingLanguages ?? [];
		var mentorLanguages = mentor.ProgrammingLanguages ?? [];
		score += preferredLanguages.Intersect(mentorLanguages, StringComparer.OrdinalIgnoreCase).Count() * 2;
		var preferredSkills = pref?.DesiredSkills ?? [];
		var mentorSkills = mentor.Skills ?? [];
		score += preferredSkills.Intersect(mentorSkills, StringComparer.OrdinalIgnoreCase).Count() * 1.25f;

		var hasIndustryMatch = pref?.DesiredIndustries.GetCategories().Split(',').Intersect(mentor.Industry.GetCategories().Split(','), StringComparer.OrdinalIgnoreCase).Any();
		if (pref is not null && hasIndustryMatch.Value)
		{
			score += 3;
		}

		if (pref is not null && mentor.ExperienceYears >= pref.MinMentorExperienceYears)
		{
			score++;
		}

		if (pref?.DesiredSkills != null)
		{
			var matchedSkills = pref.DesiredSkills.Intersect(mentor.Skills, StringComparer.OrdinalIgnoreCase).Count();
			score += matchedSkills * 0.75f;
		}

		if ( pref is null || string.IsNullOrEmpty(pref.Position) )
		{
			return score;
		}

		var positionTokens = pref.Position.Split([' ', ','], StringSplitOptions.RemoveEmptyEntries).ToList();
		score += positionTokens.Intersect(mentor.Position.Split([' ', ','], StringSplitOptions.RemoveEmptyEntries), StringComparer.OrdinalIgnoreCase).Count() * 0.5f;

		return score;
	}
}
