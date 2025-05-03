using Microsoft.AspNetCore.Identity;

namespace MentorSync.Users.Extensions;

public static class IdentityResultExtensions
{
    public static string GetErrorMessage(this IdentityResult result)
    {
        return result.Errors.Any() ? string.Join(", ", result.Errors.Select(e => e.Description)) : "An unknown error occurred.";
    }
}
