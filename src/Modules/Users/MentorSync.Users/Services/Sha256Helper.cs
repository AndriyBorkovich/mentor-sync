using System.Buffers.Text;
using System.Security.Cryptography;
using System.Text;

namespace MentorSync.Users.Services;

public static class Sha256Helper
{
    public static string ComputeHash(string input)
    {
        var challengeBytes = SHA256.HashData(Encoding.UTF8.GetBytes(input));
        var codeChallenge = Base64Url.EncodeToString(challengeBytes);
        return codeChallenge;
    }
}
