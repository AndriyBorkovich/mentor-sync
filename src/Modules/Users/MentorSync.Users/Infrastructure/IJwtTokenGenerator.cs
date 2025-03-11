using MentorSync.Users.Domain;
using MentorSync.Users.Domain.User;

namespace MentorSync.Users.Infrastructure;

public interface IJwtTokenGenerator
{
    Task<TokenResult> GenerateToken(AppUser user);
}