using MentorSync.Recommendations.Data;
using MentorSync.Recommendations.Domain;
using MentorSync.Recommendations.Infrastructure.MachineLearning.Input;
using MentorSync.Recommendations.Infrastructure.MachineLearning.Output;
using Microsoft.Extensions.Logging;
using Microsoft.ML;
using Microsoft.EntityFrameworkCore;
using MentorSync.Users.Contracts.Models;
using MentorSync.Users.Contracts.Services;

namespace MentorSync.Recommendations.Features.Pipeline;

/// <summary>
/// loads model, predicts, computes CBF, and stores final scores
/// </summary>
public interface IHybridScorer
{
    Task GenerateRecommendationsAsync(CancellationToken cancellationToken);
}

public class HybridScorer(
    RecommendationsDbContext db,
    IMenteeProfileService menteeProfileService,
    IMentorProfileService mentorProfileService,
    ILogger<HybridScorer> logger) : IHybridScorer
{
    private readonly MLContext _mlContext = new();
    private ITransformer _model;

    public async Task GenerateRecommendationsAsync(CancellationToken cancellationToken)
    {
        _model = _mlContext.Model.Load("model.zip", out _);
        logger.LogInformation("Generating hybrid recommendations...");

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
                var finalScore = (normalizedCfScore * 0.7f) + (cbfScore * 0.3f);

                db.RecommendationResults.Add(new RecommendationResult
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

        await db.SaveChangesAsync(cancellationToken);
        logger.LogInformation("Hybrid recommendations saved.");
    }

    private static float CalculateCBFScore(MenteePreferences pref, MentorProfileModel mentor)
    {
        float score = 0;
        var prefLangs = pref?.DesiredProgrammingLanguages ?? [];
        var mentorLangs = mentor?.ProgrammingLanguages ?? [];
        score += prefLangs.Intersect(mentorLangs).Count() * 2;
        if (pref is not null && pref.DesiredIndustries.HasFlag(mentor.Industry)) score += 3;
        if (pref is not null && mentor?.ExperienceYears >= pref.MinMentorExperienceYears) score += 1;

        return score;
    }
}
