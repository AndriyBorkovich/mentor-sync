namespace MentorSync.Recommendations.Domain.Tracking;

/// <summary>
/// Tracks when a mentee views a learning material (for recommendation algorithms).
/// </summary>
public sealed class MaterialViewEvent : BaseViewEvent
{
    public int MaterialId { get; set; }
}
