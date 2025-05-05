using MentorSync.Users.Contracts.Models;

namespace MentorSync.Users.Contracts.Services;

public interface IMentorProfileService
{
    Task<IEnumerable<MentorProfileModel>> GetAllMentorsAsync();
}
