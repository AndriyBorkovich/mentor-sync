namespace MentorSync.Users.Features.GetMentorBasicInfo;

public record GetMentorBasicInfoQuery(int MentorId) : IQuery<MentorBasicInfoResponse>;
