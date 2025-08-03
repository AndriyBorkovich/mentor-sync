using MentorSync.SharedKernel.CommonEntities.Enums;
using System.ComponentModel.DataAnnotations;

namespace MentorSync.Users.Domain.Base;

public class BaseProfile
{
	public int Id { get; set; }
	[StringLength(2000)]
	public required string Bio { get; set; }
	[StringLength(100)]
	public required string Position { get; set; }
	[StringLength(100)]
	public string Company { get; set; }
	public required Industry Industries { get; set; }
	[Length(1, 20)]
	public List<string> Skills { get; set; }
	[Length(1, 10)]
	public List<string> ProgrammingLanguages { get; set; }
}
