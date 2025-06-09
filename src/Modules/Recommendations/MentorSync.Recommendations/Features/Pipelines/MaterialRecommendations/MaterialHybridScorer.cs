using MentorSync.Materials.Contracts.Models;
using MentorSync.Materials.Contracts.Services;
using MentorSync.Recommendations.Data;
using MentorSync.Recommendations.Domain.Result;
using MentorSync.Recommendations.Features.Pipelines.Base;
using MentorSync.Recommendations.Infrastructure.MachineLearning.Input;
using MentorSync.Recommendations.Infrastructure.MachineLearning.Output;
using MentorSync.SharedKernel.Extensions;
using MentorSync.Users.Contracts.Models;
using MentorSync.Users.Contracts.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.ML;

namespace MentorSync.Recommendations.Features.Pipelines.MaterialRecommendations;

public sealed class MaterialHybridScorer(
    RecommendationsDbContext db,
    IMenteeProfileService menteeProfileService,
    ILearningMaterialsService learningMaterialsService,
    ILogger<MaterialHybridScorer> logger) : IHybridScorer
{
    private readonly MLContext _mlContext = new();
    private ITransformer _model;

    public async Task GenerateRecommendationsAsync(CancellationToken cancellationToken)
    {
        _model = _mlContext.Model.Load("material_model.zip", out _);
        logger.LogInformation("Generating hybrid recommendations for learning materials...");

        var menteePreferences = await menteeProfileService.GetMenteesPreferences();
        var materials = await learningMaterialsService.GetAllMaterialsAsync(cancellationToken);

        var engine = _mlContext.Model.CreatePredictionEngine<MenteeMaterialRatingData, MaterialPrediction>(_model);

        foreach (var preferences in menteePreferences)
        {
            foreach (var material in materials)
            {
                var cfScore = engine.Predict(new MenteeMaterialRatingData
                {
                    MenteeId = preferences.MenteeId.ToString(),
                    MaterialId = material.Id.ToString()
                }).Score;

                var normalizedCfScore = float.IsNaN(cfScore) ? 0 : cfScore;
                var cbfScore = CalculateCBFScore(preferences, material);
                var finalScore = normalizedCfScore * 0.6f + cbfScore * 0.4f;

                var existingRec = await db.MaterialRecommendationResults
                    .FirstOrDefaultAsync(r =>
                        r.MenteeId == preferences.MenteeId &&
                        r.MaterialId == material.Id,
                        cancellationToken);

                if (existingRec != null)
                {
                    existingRec.CollaborativeScore = normalizedCfScore;
                    existingRec.ContentBasedScore = cbfScore;
                    existingRec.FinalScore = finalScore;
                    existingRec.GeneratedAt = DateTime.UtcNow;
                    db.MaterialRecommendationResults.Update(existingRec);
                }
                else
                {
                    db.MaterialRecommendationResults.Add(new MaterialRecommendationResult
                    {
                        MenteeId = preferences.MenteeId,
                        MaterialId = material.Id,
                        CollaborativeScore = normalizedCfScore,
                        ContentBasedScore = cbfScore,
                        FinalScore = finalScore,
                        GeneratedAt = DateTime.UtcNow
                    });
                }
            }
        }

        await db.SaveChangesAsync(cancellationToken);
        logger.LogInformation("Hybrid recommendations for learning materials saved.");
    }

    /// <summary>
    /// Calculate content-based filtering score based on the overlap between mentee preferences and material properties
    /// </summary>
    private static float CalculateCBFScore(MenteePreferences pref, LearningMaterialModel material)
    {
        float score = 0;

        var industries = pref.DesiredIndustries.GetCategories().Split(',');

        var matchingTitleTokens = material.Title.Split(' ').Intersect(industries, StringComparer.OrdinalIgnoreCase).Count();
        score += matchingTitleTokens;

        var matchingDescriptionTokens = material.Description.Split(' ').Intersect(industries, StringComparer.OrdinalIgnoreCase).Count();
        score += matchingDescriptionTokens;

        var matchingTopics = industries.Intersect(material.Tags, StringComparer.OrdinalIgnoreCase).Count();
        score += matchingTopics * 2;

        // Check if material is in one of mentee's programming languages
        if (material.Tags.Any(tag => pref.DesiredProgrammingLanguages.Contains(tag, StringComparer.OrdinalIgnoreCase)))
            score += 2;

        // Consider recency (newer materials get slightly higher scores)
        var daysOld = (DateTime.UtcNow - material.CreatedAt).TotalDays;
        if (daysOld < 30) // Less than a month old
            score += 1;

        return score;
    }
}
