using MentorSync.Users.Contracts.Models;

namespace MentorSync.Users.Contracts.Services;

public interface IMenteeProfileService
{
	Task<UserBasicInfoModel> GetMenteeInfo(int menteeId);
	Task<IReadOnlyList<MenteePreferences>> GetMenteesPreferences();
}
