namespace MentorSync.Recommendations.Domain.Tracking;

/// <summary>
/// Tracks mentees bookmarking mentor profiles.
/// </summary>
public sealed class MentorBookmark
{
    public int Id { get; set; }
    /// <summary>
    /// The ID of the mentee who performed the bookmark.
    /// </summary>
    public int MenteeId { get; set; }
    /// <summary>
    /// The ID of the mentor who was bookmarked.
    /// </summary>
    public int MentorId { get; set; }
    /// <summary>
    /// Date and time when the bookmark event occurred.
    /// </summary>
    public DateTime BookmarkedAt { get; set; } = DateTime.UtcNow;
}
