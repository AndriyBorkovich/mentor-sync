using MentorSync.SharedKernel.CommonEntities;
using System.ComponentModel.DataAnnotations;

namespace MentorSync.Users.Domain.Base;

public class BaseProfile
{
    public int Id { get; set; }
    public Industry Industries { get; set; }
    [Length(1, 20)]
    public List<string> Skills { get; set; }
    [Length(1, 10)]
    public List<string> ProgrammingLanguages { get; set; }
}
