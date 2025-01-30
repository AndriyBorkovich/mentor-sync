using System.ComponentModel.DataAnnotations;
using MentorSync.Users.Domain.Enums;

namespace MentorSync.Users.Domain;

public sealed class MenteeProfile
{
    public int Id { get; set; }
    [Length(1, 20)]
    public List<string> DesiredSkills { get; set; }
    [StringLength(200)]
    public string LearningGoals { get; set; }
    public Industry PreferredMentorIndustry { get; set; }
    [Length(1, 5)]
    public List<string> PreferredLanguages { get; set; }
    
    public int MenteeId { get; set; }
    public AppUser User { get; set; }
}