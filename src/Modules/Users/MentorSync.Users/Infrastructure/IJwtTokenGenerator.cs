using MentorSync.Users.Domain;

namespace MentorSync.Users.Infrastructure;

public interface IJwtTokenGenerator
{
    Task<TokenResult> GenerateToken(AppUser user);
}