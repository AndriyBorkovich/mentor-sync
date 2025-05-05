using MentorSync.Users.Contracts.Models;
using MentorSync.Users.Contracts.Services;

namespace MentorSync.Users.Services;

public sealed class MentorProfileService : IMentorProfileService
{
    public Task<IEnumerable<MentorProfileModel>> GetAllMentorsAsync() =>
        Task.FromResult<IEnumerable<MentorProfileModel>>([]);
}
