using MentorSync.Recommendations.Data;
using MentorSync.Recommendations.Domain.Preferences;
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

public class HybridScorer : IHybridScorer
{
    private readonly RecommendationDbContext _db;
    private readonly ILogger<HybridScorer> _logger;
    private readonly IMentorProfileService _mentorProfileService;
    private readonly MLContext _mlContext = new();
    private readonly ITransformer _model;

    public HybridScorer(
        RecommendationDbContext db,
        IMentorProfileService mentorProfileService,
        ILogger<HybridScorer> logger)
    {
        _db = db;
        _mentorProfileService = mentorProfileService;
        _logger = logger;
        _model = _mlContext.Model.Load("model.zip", out _);
    }

    public async Task GenerateRecommendationsAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Generating hybrid recommendations...");

        var mentees = await _db.MenteePreferences.Select(x => x.MenteeId).ToListAsync(cancellationToken);
        var mentors = await _mentorProfileService.GetAllMentorsAsync();

        var engine = _mlContext.Model.CreatePredictionEngine<MenteeMentorRatingData, MentorPrediction>(_model);

        foreach (var menteeId in mentees)
        {
            var preferences = await _db.MenteePreferences.FirstAsync(x => x.MenteeId == menteeId, cancellationToken);
            foreach (var mentor in mentors)
            {
                var cfScore = engine.Predict(new MenteeMentorRatingData
                {
                    MenteeId = menteeId.ToString(),
                    MentorId = mentor.Id.ToString()
                }).Score;

                var cbfScore = CalculateCBFScore(preferences, mentor);
                var finalScore = (cfScore * 0.7f) + (cbfScore * 0.3f);

                _db.RecommendationResults.Add(new RecommendationResult
                {
                    MenteeId = menteeId,
                    MentorId = mentor.Id,
                    CollaborativeScore = cfScore,
                    ContentBasedScore = cbfScore,
                    FinalScore = finalScore,
                    GeneratedAt = DateTime.UtcNow
                });
            }
        }

        await _db.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("Hybrid recommendations saved.");
    }

    private static float CalculateCBFScore(MenteePreference pref, MentorProfileModel mentor)
    {
        float score = 0;
        var prefLangs = pref.DesiredProgrammingLanguages;
        var mentorLangs = mentor.ProgrammingLanguages;
        score += prefLangs.Intersect(mentorLangs).Count() * 2;
        if (pref.DesiredIndustries.Contains(mentor.Industry)) score += 3;
        if (mentor.ExperienceYears >= pref.MinMentorExperienceYears) score += 1;
        if (mentor.CommunicationLanguage == pref.PreferredCommunicationLanguage) score += 1;
        return score;
    }
}
