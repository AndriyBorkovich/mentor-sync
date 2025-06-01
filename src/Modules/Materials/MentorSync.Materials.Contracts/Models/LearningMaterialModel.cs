using MentorSync.SharedKernel.CommonEntities;

namespace MentorSync.Materials.Contracts.Models;

/// <summary>
/// Model representing a learning material for recommendation purposes.
/// </summary>
public sealed class LearningMaterialModel
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public MaterialType Type { get; set; }
    public int MentorId { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<string> Tags { get; set; } = [];
}
