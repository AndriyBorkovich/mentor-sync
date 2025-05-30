using MentorSync.Users.Contracts.Models;

namespace MentorSync.Users.Contracts.Services;

public interface IMenteeProfileService
{
    Task<MenteeBasicInfoModel> GetMenteeInfo(int menteeId);
    Task<List<MenteePreferences>> GetMenteesPreferences();
}
