using MentorSync.Users.Domain;
using MentorSync.Users.Domain.User;
using MentorSync.Users.Features.Bio.Search;

namespace MentorSync.Users.MappingExtensions;

public static class DomainToDtoMapper
{
    public static SearchUserByBioResponse ToSearchByBioResponse(this AppUser user)
    {
        return new SearchUserByBioResponse(user.Id, user.Bio);
    }
}