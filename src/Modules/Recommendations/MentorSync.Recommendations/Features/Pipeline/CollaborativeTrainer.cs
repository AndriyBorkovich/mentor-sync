using MentorSync.Recommendations.Data;
using MentorSync.Recommendations.Infrastructure.MachineLearning.Input;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.ML;
using Microsoft.ML.Trainers;

namespace MentorSync.Recommendations.Features.Pipeline;

/// <summary>
/// trains and saves the ML.NET model
/// </summary>
public interface ICollaborativeTrainer
{
    Task TrainAsync(CancellationToken cancellationToken);
}

public class CollaborativeTrainer : ICollaborativeTrainer
{
    private readonly RecommendationsDbContext _db;
    private readonly ILogger<CollaborativeTrainer> _logger;
    private readonly MLContext _mlContext = new();

    public CollaborativeTrainer(RecommendationsDbContext db, ILogger<CollaborativeTrainer> logger)
    {
        _db = db;
        _logger = logger;
    }

    public async Task TrainAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Training collaborative model...");

        var data = await _db.MenteeMentorInteractions.ToListAsync(cancellationToken);

        var mlData = _mlContext.Data.LoadFromEnumerable(data.Select(x => new MenteeMentorRatingData
        {
            MenteeId = x.MenteeId.ToString(),
            MentorId = x.MentorId.ToString(),
            Label = x.Score
        }));

        var options = new MatrixFactorizationTrainer.Options
        {
            MatrixColumnIndexColumnName = nameof(MenteeMentorRatingData.MenteeId),
            MatrixRowIndexColumnName = nameof(MenteeMentorRatingData.MentorId),
            LabelColumnName = nameof(MenteeMentorRatingData.Label),
            NumberOfIterations = 20,
            ApproximationRank = 100
        };

        var trainer = _mlContext.Recommendation().Trainers.MatrixFactorization(options);
        var model = trainer.Fit(mlData);

        _mlContext.Model.Save(model, mlData.Schema, "model.zip");

        _logger.LogInformation("Collaborative model trained and saved.");
    }
}

