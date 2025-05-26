using MentorSync.Users.Domain.Mentee;
using MentorSync.Users.Domain.Mentor;
using MentorSync.Users.Features.Mentee.CreateProfile;
using MentorSync.Users.Features.Mentee.EditProfile;
using MentorSync.Users.Features.Mentor.CreateProfile;
using MentorSync.Users.Features.Mentor.EditProfile;

namespace MentorSync.Users.MappingExtensions;

public static class DtoToDomainMapper
{
    public static MentorProfile ToMentorProfile(this CreateMentorProfileCommand command)
    {
        return new MentorProfile
        {
            Bio = command.Bio,
            Position = command.Position,
            Company = command.Company,
            Industries = command.Industries,
            Skills = command.Skills,
            ProgrammingLanguages = command.ProgrammingLanguages,
            ExperienceYears = command.ExperienceYears,
            Availability = command.Availability,
            MentorId = command.MentorId
        };
    }

    public static MenteeProfile ToMenteeProfile(this CreateMenteeProfileCommand command)
    {
        return new MenteeProfile
        {
            Bio = command.Bio,
            Position = command.Position,
            Company = command.Company,
            Industries = command.Industries,
            Skills = command.Skills,
            ProgrammingLanguages = command.ProgrammingLanguages,
            LearningGoals = command.LearningGoals,
            MenteeId = command.MenteeId
        };
    }
    public static void UpdateFrom(this MentorProfile profile, EditMentorProfileCommand command)
    {
        profile.Bio = command.Bio;
        profile.Position = command.Position;
        profile.Company = command.Company;
        profile.Industries = command.Industries;
        profile.Skills = command.Skills;
        profile.ProgrammingLanguages = command.ProgrammingLanguages;
        profile.ExperienceYears = command.ExperienceYears;
        profile.Availability = command.Availability;
    }

    public static void UpdateFrom(this MenteeProfile profile, EditMenteeProfileCommand command)
    {
        profile.Bio = command.Bio;
        profile.Position = command.Position;
        profile.Company = command.Company;
        profile.Industries = command.Industries;
        profile.Skills = command.Skills;
        profile.ProgrammingLanguages = command.ProgrammingLanguages;
        profile.LearningGoals = command.LearningGoals;
    }
}
