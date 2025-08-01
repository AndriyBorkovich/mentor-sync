﻿using MentorSync.Recommendations.Data;
using MentorSync.Recommendations.Infrastructure.MachineLearning.Input;
using MentorSync.Recommendations.Infrastructure.MachineLearning.Output;
using Microsoft.Extensions.Logging;
using Microsoft.ML;
using Microsoft.EntityFrameworkCore;
using MentorSync.Users.Contracts.Models;
using MentorSync.Users.Contracts.Services;
using MentorSync.Recommendations.Features.Pipelines.Base;
using MentorSync.Recommendations.Domain.Result;
using MentorSync.SharedKernel.Extensions;

namespace MentorSync.Recommendations.Features.Pipelines.MentorRecommendations;

public sealed class MentorHybridScorer(
    RecommendationsDbContext db,
    IMenteeProfileService menteeProfileService,
    IMentorProfileService mentorProfileService,
    ILogger<MentorHybridScorer> logger) : IHybridScorer
{
    private readonly MLContext _mlContext = new();
    private ITransformer _model;

    public async Task GenerateRecommendationsAsync(CancellationToken cancellationToken)
    {
        _model = _mlContext.Model.Load(ServicesConstants.MentorModelFile, out _);
        logger.LogInformation("Generating hybrid recommendations for mentors...");

        var menteePreferences = await menteeProfileService.GetMenteesPreferences();
        var mentors = await mentorProfileService.GetAllMentorsAsync();

        var engine = _mlContext.Model.CreatePredictionEngine<MenteeMentorRatingData, MentorPrediction>(_model);

        foreach (var preferences in menteePreferences)
        {
            foreach (var mentor in mentors)
            {
                var cfScore = engine.Predict(new MenteeMentorRatingData
                {
                    MenteeId = preferences.MenteeId.ToString(),
                    MentorId = mentor.Id.ToString()
                }).Score;

                var normalizedCfScore = float.IsNaN(cfScore) ? 0 : cfScore;
                var cbfScore = CalculateCBFScore(preferences, mentor);
                var finalScore = normalizedCfScore * 0.7f + cbfScore * 0.3f;

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
        }

        await db.SaveChangesAsync(cancellationToken);
        logger.LogInformation("Hybrid recommendations saved.");
    }

    private static float CalculateCBFScore(MenteePreferences pref, MentorProfileModel mentor)
    {
        float score = 0;
        var prefLangs = pref?.DesiredProgrammingLanguages ?? [];
        var mentorLangs = mentor?.ProgrammingLanguages ?? [];
        score += prefLangs.Intersect(mentorLangs).Count() * 2;

        var prefSkills = pref?.DesiredSkills ?? [];
        var mentorSkills = mentor?.Skills ?? [];
        score += prefSkills.Intersect(mentorSkills).Count() * 1.25f;

        var hasIndustryMatch = pref?.DesiredIndustries.GetCategories().Split(',').Intersect(mentor?.Industry.GetCategories().Split(','), StringComparer.OrdinalIgnoreCase).Count() > 0;

        if (pref is not null && hasIndustryMatch) score += 3;
        if (pref is not null && mentor?.ExperienceYears >= pref.MinMentorExperienceYears) score += 1;
        if (pref is not null && pref.DesiredSkills != null)
        {
            var matchedSkills = pref.DesiredSkills.Intersect(mentor.Skills).Count();
            score += matchedSkills * 0.75f;
        }

        if (pref is not null && !string.IsNullOrEmpty(pref.Position))
        {
            var positionTokens = pref.Position.Split([' ', ','], StringSplitOptions.RemoveEmptyEntries).ToList();
            // intersect checks if any of the position tokens match the mentor's position
            score += positionTokens.Intersect(mentor.Position.Split([' ', ','], StringSplitOptions.RemoveEmptyEntries), StringComparer.OrdinalIgnoreCase).Count() * 0.5f;
        }

        return score;
    }
}
