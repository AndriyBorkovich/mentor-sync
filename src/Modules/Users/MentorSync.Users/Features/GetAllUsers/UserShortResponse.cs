namespace MentorSync.Users.Features.GetAllUsers
{
    public sealed record UserShortResponse(
        int Id,
        string Name,
        string Email,
        string Role,
        string AvatarUrl,
        bool IsActive,
        bool IsEmailConfirmed);
}
