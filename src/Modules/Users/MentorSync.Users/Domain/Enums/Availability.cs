namespace MentorSync.Users.Domain.Enums;

/// <summary>
/// Represents the availability of a mentor during different times of the day.
/// </summary>
[Flags]
public enum Availability
{
	/// <summary>
	/// No availability
	/// </summary>
	None = 0,
	/// <summary>
	/// Available in the morning
	/// </summary>
	Morning = 1 << 0, // 1
	/// <summary>
	/// Available in the afternoon
	/// </summary>
	Afternoon = 1 << 1, // 2
	/// <summary>
	/// Available in the evening
	/// </summary>
	Evening = 1 << 2, // 4
	/// <summary>
	/// Available at night
	/// </summary>
	Night = 1 << 3  // 8
}
