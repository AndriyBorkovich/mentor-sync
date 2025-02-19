using Ardalis.Result;
using MediatR;

namespace MentorSync.Users.Features.Bio.Add;

public sealed record AddBioRequest(int UserId, string Bio) : IRequest<Result<string>>;
