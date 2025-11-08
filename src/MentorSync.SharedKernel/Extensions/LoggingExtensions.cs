namespace MentorSync.SharedKernel.Extensions;

/// <summary>
/// Extension methods for safe logging operations
/// </summary>
public static class LoggingExtensions
{
	/// <summary>
	/// Sanitizes a string value for safe logging by removing newline and carriage return characters
	/// that could be used for log injection attacks.
	/// </summary>
	/// <param name="value">The string value to sanitize</param>
	/// <returns>The sanitized string, or null if the input is null</returns>
	/// <example>
	/// <code>
	/// var sanitized = LoggingExtensions.SanitizeForLogging(userEmail);
	/// logger.LogWarning("Login failed for {Email}", sanitized);
	/// </code>
	/// </example>
	public static string? SanitizeForLogging(string? value)
	{
		return value?.Replace("\r", "").Replace("\n", "");
	}
}
