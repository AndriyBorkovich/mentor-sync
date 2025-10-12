using Microsoft.AspNetCore.Identity;

namespace MentorSync.Users.Extensions;

/// <summary>
/// Extension methods for IdentityResult
/// </summary>
public static class IdentityResultExtensions
{
	/// <summary>
	/// Gets a concatenated error message from an IdentityResult
	/// </summary>
	/// <param name="result">Extended type</param>
	/// <returns>Formatted string</returns>
	public static string GetErrorMessage(this IdentityResult result)
	{
		return result.Errors.Any() ? string.Join(", ", result.Errors.Select(e => e.Description)) : "An unknown error occurred.";
	}
}
