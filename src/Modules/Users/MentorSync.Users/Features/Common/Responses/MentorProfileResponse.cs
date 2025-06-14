using MentorSync.SharedKernel.CommonEntities;

namespace MentorSync.Users.Features.Common.Responses;

public sealed record MentorProfileResponse(int Id,
                                           Industry Industries,
                                           List<string> Skills,
                                           List<string> ProgrammingLanguages,
                                           int ExperienceYears,
                                           string Availability,
                                           int MentorId);
