using Ardalis.Result;
using MediatR;
using MentorSync.SharedKernel.CommonEntities.Enums;
using MentorSync.Users.Domain.Enums;
using MentorSync.Users.Features.Common.Responses;

namespace MentorSync.Users.Features.Mentor.EditProfile;

public sealed record EditMentorProfileCommand(
    int Id,
    string Bio,
    string Position,
    string Company,
    Industry Industries,
    List<string> Skills,
    List<string> ProgrammingLanguages,
    int ExperienceYears,
    Availability Availability,
    int MentorId
) : IRequest<Result<MentorProfileResponse>>;
