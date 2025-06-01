using MentorSync.Materials.Contracts.Models;

namespace MentorSync.Materials.Contracts.Services;

/// <summary>
/// Service for accessing learning materials data for recommendation algorithms.
/// </summary>
public interface ILearningMaterialsService
{
    /// <summary>
    /// Get all learning materials for recommendation processing.
    /// </summary>
    Task<List<LearningMaterialModel>> GetAllMaterialsAsync(CancellationToken cancellationToken = default);
}
