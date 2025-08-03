namespace MentorSync.Users.Domain.Enums;

[Flags]
public enum Availability
{
	None = 0,
	Morning = 1 << 0, // 1
	Afternoon = 1 << 1, // 2
	Evening = 1 << 2, // 4
	Night = 1 << 3  // 8
}