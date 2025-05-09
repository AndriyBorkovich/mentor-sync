namespace MentorSync.Recommendations.Domain.Tracking;

/// <summary>
/// Tracks mentees viewing mentor profiles.
/// </summary>
public sealed class MentorViewEvent
{
    public int Id { get; set; }
    /// <summary>
    /// The ID of the mentee who perfomed view.
    /// </summary>
    public int MenteeId { get; set; }
    /// <summary>
    /// The ID of the mentor who was viewed.
    /// </summary>
    public int MentorId { get; set; }
    /// <summary>
    /// Date and time when the view event occurred.
    /// </summary>
    public DateTime ViewedAt { get; set; } = DateTime.UtcNow;
}
