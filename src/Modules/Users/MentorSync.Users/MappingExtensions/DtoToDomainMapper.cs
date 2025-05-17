using MentorSync.Users.Domain.Mentor;
using MentorSync.Users.Features.Mentor.CreateProfile;
using MentorSync.Users.Features.Mentor.EditProfile;

namespace MentorSync.Users.MappingExtensions;

public static class DtoToDomainMapper
{
    public static MentorProfile ToMentorProfile(this CreateMentorProfileCommand command)
    {
        return new MentorProfile
        {
            Industries = command.Industries,
            Skills = command.Skills,
            ProgrammingLanguages = command.ProgrammingLanguages,
            ExperienceYears = command.ExperienceYears,
            Availability = command.Availability,
            MentorId = command.MentorId
        };
    }

    public static void UpdateFrom(this MentorProfile profile, EditMentorProfileCommand command)
    {
        profile.Industries = command.Industries;
        profile.Skills = command.Skills;
        profile.ProgrammingLanguages = command.ProgrammingLanguages;
        profile.ExperienceYears = command.ExperienceYears;
        profile.Availability = command.Availability;
    }
}
