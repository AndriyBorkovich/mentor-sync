using MentorSync.SharedKernel.CommonEntities.Enums;

namespace MentorSync.Users.Features.Common.Responses;

public sealed record MentorProfileResponse(int Id,
										   Industry Industries,
										   IReadOnlyList<string> Skills,
										   IReadOnlyList<string> ProgrammingLanguages,
										   int ExperienceYears,
										   string Availability,
										   int MentorId);
