namespace MentorSync.Users.Features.GetMentorBasicInfo;

/// <summary>
/// Query to get basic information about a mentor by their ID
/// </summary>
public sealed record GetMentorBasicInfoQuery(int MentorId) : IQuery<MentorBasicInfoResponse>;
