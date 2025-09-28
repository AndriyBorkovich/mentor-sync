using System.Security.Claims;
using MentorSync.Users.Domain.User;

namespace MentorSync.Users.Infrastructure;

/// <summary>
/// Service interface for JWT token generation and validation
/// </summary>
public interface IJwtTokenService
{
	/// <summary>
	/// Generates a JWT token for the specified user
	/// </summary>
	/// <param name="user">The user to generate a token for</param>
	/// <returns>A task that represents the asynchronous operation, containing the token result</returns>
	Task<TokenResult> GenerateToken(AppUser user);

	/// <summary>
	/// Extracts the claims principal from an expired JWT token
	/// </summary>
	/// <param name="token">The expired JWT token</param>
	/// <returns>The claims principal extracted from the token</returns>
	ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
}
