using Ardalis.Result;
using MediatR;

namespace MentorSync.Users.Features.GetMentorBasicInfo;

public record GetMentorBasicInfoQuery(int MentorId) : IRequest<Result<MentorBasicInfoResponse>>;
