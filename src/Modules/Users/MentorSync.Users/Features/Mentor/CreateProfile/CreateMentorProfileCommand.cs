﻿using Ardalis.Result;
using MediatR;
using MentorSync.SharedKernel.CommonEntities;
using MentorSync.Users.Domain.Enums;
using MentorSync.Users.Features.Common.Responses;

namespace MentorSync.Users.Features.Mentor.CreateProfile;

public sealed record CreateMentorProfileCommand(
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
