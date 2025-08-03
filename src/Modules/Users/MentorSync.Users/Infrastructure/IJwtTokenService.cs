using System.Security.Claims;
using MentorSync.Users.Domain.User;

namespace MentorSync.Users.Infrastructure;

public interface IJwtTokenService
{
	Task<TokenResult> GenerateToken(AppUser user);
	ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
}
