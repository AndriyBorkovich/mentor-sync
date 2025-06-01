namespace MentorSync.Recommendations.Domain.Tracking;

/// <summary>
/// Tracks when a mentee likes a learning material (for recommendation algorithms).
/// </summary>
public sealed class MaterialLike
{
    public int Id { get; set; }
    public int MenteeId { get; set; }
    public int MaterialId { get; set; }
    public DateTime LikedAt { get; set; } = DateTime.UtcNow;
}
