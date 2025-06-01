namespace MentorSync.Recommendations.Domain.Tracking;

public class BaseViewEvent
{
    public int Id { get; set; }
    public int MenteeId { get; set; }
    public DateTime ViewedAt { get; set; } = DateTime.UtcNow;
}
