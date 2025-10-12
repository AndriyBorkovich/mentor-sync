using System.Globalization;
using MentorSync.Recommendations.Data;
using MentorSync.Recommendations.Features.Pipelines.Base;
using MentorSync.Recommendations.Infrastructure.MachineLearning.Input;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.ML;
using Microsoft.ML.Trainers;

namespace MentorSync.Recommendations.Features.Pipelines.MentorRecommendations;

/// <summary>
/// Trains a collaborative filtering model for mentor recommendations using ML.NET
/// </summary>
/// <param name="db">Database context</param>
/// <param name="logger">Logger</param>
public sealed class MentorCollaborativeTrainer(RecommendationsDbContext db, ILogger<MentorCollaborativeTrainer> logger) : ICollaborativeTrainer
{
	private readonly MLContext _mlContext = new();

	/// <inheritdoc />
	public async Task TrainAsync(CancellationToken cancellationToken)
	{
		logger.LogInformation("Training collaborative model...");

		var pipeline = _mlContext.Transforms.Conversion
		.MapValueToKey(
			outputColumnName: "MenteeKey",
			inputColumnName: nameof(MenteeMentorRatingData.MenteeId))
		.Append(_mlContext.Transforms.Conversion
		.MapValueToKey(
			outputColumnName: "MentorKey",
			inputColumnName: nameof(MenteeMentorRatingData.MentorId)))
		.Append(_mlContext.Recommendation().Trainers.MatrixFactorization(new MatrixFactorizationTrainer.Options
		{
			MatrixColumnIndexColumnName = "MenteeKey",
			MatrixRowIndexColumnName = "MentorKey",
			LabelColumnName = nameof(MenteeMentorRatingData.Label),
			NumberOfIterations = 30,
			ApproximationRank = 100
		}));

		var data = await db.MenteeMentorInteractions.ToListAsync(cancellationToken);

		var mlData = _mlContext.Data.LoadFromEnumerable(data.Select(x => new MenteeMentorRatingData
		{
			MenteeId = x.MenteeId.ToString(CultureInfo.InvariantCulture),
			MentorId = x.MentorId.ToString(CultureInfo.InvariantCulture),
			Label = x.Score
		}));

		var model = pipeline.Fit(mlData);

		_mlContext.Model.Save(model, mlData.Schema, ServicesConstants.MentorModelFile);

		logger.LogInformation("Collaborative model trained and saved.");
	}
}

