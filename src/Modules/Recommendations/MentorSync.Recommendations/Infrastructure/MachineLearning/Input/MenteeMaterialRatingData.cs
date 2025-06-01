using Microsoft.ML.Data;

namespace MentorSync.Recommendations.Infrastructure.MachineLearning.Input;

public sealed class MenteeMaterialRatingData
{
    [LoadColumn(0)]
    public string MenteeId { get; set; } = default!;

    [LoadColumn(1)]
    public string MaterialId { get; set; } = default!;

    [LoadColumn(2)]
    public float Label { get; set; } // Interaction score
}
