namespace MentorSync.Users.Features.GetUserProfile;

public sealed record UserProfileResponse(
    int Id,
    string UserName,
    string Email,
    string Role,
    string ProfileImageUrl,
    bool IsActive,
    MenteeProfileInfo MenteeProfile,
    MentorProfileInfo MentorProfile
);

public sealed record MenteeProfileInfo(
    int Id,
    string Bio,
    string Position,
    string Company,
    List<string> Skills,
    List<string> ProgrammingLanguages,
    List<string> LearningGoals
);

public sealed record MentorProfileInfo(
    int Id,
    string Bio,
    string Position,
    string Company,
    List<string> Skills,
    List<string> ProgrammingLanguages,
    int ExperienceYears
);
