using MentorSync.Users.Domain.User;
using MentorSync.Users.Features.GetAllUsers;

namespace MentorSync.Users.MappingExtensions;

public static class DomainToDtoMapper
{
    public static UserShortResponse ToUserShortResponse(this AppUser user)
    {
        return new UserShortResponse(
            user.Id,
            user.UserName,
            user.Email,
            user.UserRoles?.FirstOrDefault()?.Role?.Name,
            user.ProfileImageUrl,
            user.IsActive,
            user.EmailConfirmed);
    }
}
