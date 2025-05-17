using Ardalis.Result;
using MediatR;
using MentorSync.SharedKernel.CommonEntities;
using MentorSync.Users.Domain.Enums;
using MentorSync.Users.Features.Common.Responses;

namespace MentorSync.Users.Features.Mentor.EditProfile;

public sealed record EditMentorProfileCommand(
    int Id,
    Industry Industries,
    List<string> Skills,
    List<string> ProgrammingLanguages,
    int ExperienceYears,
    Availability Availability,
    int MentorId
) : IRequest<Result<MentorProfileResponse>>;
