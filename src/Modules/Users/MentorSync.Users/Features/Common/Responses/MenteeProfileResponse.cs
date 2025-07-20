using MentorSync.SharedKernel.CommonEntities.Enums;

namespace MentorSync.Users.Features.Common.Responses;

public sealed record MenteeProfileResponse(int Id,
                                           string Bio,
                                           string Position,
                                           string Company,
                                           Industry Industries,
                                           List<string> Skills,
                                           List<string> ProgrammingLanguages,
                                           List<string> LearningGoals,
                                           int MenteeId);
