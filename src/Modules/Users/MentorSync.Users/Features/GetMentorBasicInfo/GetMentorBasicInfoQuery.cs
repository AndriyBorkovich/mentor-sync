namespace MentorSync.Users.Features.GetMentorBasicInfo;

public sealed record GetMentorBasicInfoQuery(int MentorId) : IQuery<MentorBasicInfoResponse>;
