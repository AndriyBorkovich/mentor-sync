using System.ComponentModel.DataAnnotations;
using MentorSync.Users.Domain.Base;
using MentorSync.Users.Domain.User;

namespace MentorSync.Users.Domain.Mentee;

public sealed class MenteeProfile : BaseProfile
{
    [StringLength(200)]
    public List<string> LearningGoals { get; set; }

    public int MenteeId { get; set; }
    public AppUser User { get; set; }
}
