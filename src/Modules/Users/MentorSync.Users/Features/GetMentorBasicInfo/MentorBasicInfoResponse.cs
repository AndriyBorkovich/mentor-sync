namespace MentorSync.Users.Features.GetMentorBasicInfo;

public record MentorBasicInfoResponse
{
    public int Id { get; init; }
    public string Name { get; init; }
    public string Title { get; init; }
    public double Rating { get; init; }
    public int ReviewsCount { get; init; }
    public string ProfileImage { get; init; }
    public int? YearsOfExperience { get; init; }
    public string Category { get; init; }
    public string Bio { get; init; }
    public string Availability { get; init; }
    public List<string> ProgrammingLanguages { get; init; } = [];
    public List<MentorSkillDto> Skills { get; init; } = [];
}

public record MentorSkillDto
{
    public string Id { get; init; }
    public string Name { get; init; }
}
