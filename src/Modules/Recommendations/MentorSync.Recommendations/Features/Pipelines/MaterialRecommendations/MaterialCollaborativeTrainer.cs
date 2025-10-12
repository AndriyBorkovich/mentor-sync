using System.Globalization;
using MentorSync.Recommendations.Data;
using MentorSync.Recommendations.Features.Pipelines.Base;
using MentorSync.Recommendations.Infrastructure.MachineLearning.Input;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.ML;
using Microsoft.ML.Trainers;

namespace MentorSync.Recommendations.Features.Pipelines.MaterialRecommendations;

/// <inheritdoc />
public sealed class MaterialCollaborativeTrainer(
	RecommendationsDbContext db,
	ILogger<MaterialCollaborativeTrainer> logger) : ICollaborativeTrainer
{
	private readonly MLContext _mlContext = new();

	/// <inheritdoc />
	public async Task TrainAsync(CancellationToken cancellationToken)
	{
		logger.LogInformation("Training collaborative model for learning materials...");

		var pipeline = _mlContext.Transforms.Conversion
			.MapValueToKey(
				outputColumnName: "MenteeKey",
				inputColumnName: nameof(MenteeMaterialRatingData.MenteeId))
			.Append(_mlContext.Transforms.Conversion
			.MapValueToKey(
				outputColumnName: "MaterialKey",
				inputColumnName: nameof(MenteeMaterialRatingData.MaterialId)))
			.Append(_mlContext.Recommendation().Trainers.MatrixFactorization(new MatrixFactorizationTrainer.Options
			{
				MatrixColumnIndexColumnName = "MenteeKey",
				MatrixRowIndexColumnName = "MaterialKey",
				LabelColumnName = nameof(MenteeMaterialRatingData.Label),
				NumberOfIterations = 30,
				ApproximationRank = 100
			}));

		var data = await db.MenteeMaterialInteractions.ToListAsync(cancellationToken);

		var mlData = _mlContext.Data.LoadFromEnumerable(data.Select(x => new MenteeMaterialRatingData
		{
			MenteeId = x.MenteeId.ToString(CultureInfo.InvariantCulture),
			MaterialId = x.MaterialId.ToString(CultureInfo.InvariantCulture),
			Label = x.Score
		}));

		var model = pipeline.Fit(mlData);

		_mlContext.Model.Save(model, mlData.Schema, ServicesConstants.MaterialModelFile);

		logger.LogInformation("Collaborative model for learning materials trained and saved.");
	}
}
