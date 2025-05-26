using Ardalis.Result;
using MediatR;
using MentorSync.SharedKernel.CommonEntities;
using MentorSync.Users.Features.Common.Responses;

namespace MentorSync.Users.Features.Mentee.EditProfile;

public sealed record EditMenteeProfileCommand(
    int Id,
    string Bio,
    string Position,
    string Company,
    Industry Industries,
    List<string> Skills,
    List<string> ProgrammingLanguages,
    List<string> LearningGoals,
    int MenteeId
) : IRequest<Result<MenteeProfileResponse>>;
