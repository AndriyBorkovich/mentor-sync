namespace MentorSync.SharedKernel;

public static class GeneralConstants
{
    public const string DatabaseName = "MentorSyncDb";
    public const string DefaultMigrationsTableName = "MigrationsHistory";
    public const string DefaultPassword = "qadbsfzYFJHS!";
    public const string DefaultEmail = "donotreply@7e133b28-71cb-4851-8bf4-701df3a6ce78.azurecomm.net";
    public const int MinPasswordLength = 8;
    public const int MaxEmailLength = 256;
    public const int DefaultLockoutTimeInMinutes = 5;
    public const int ProtectionTokenTimeInHours = 2;
    public const int MaxFailedAccessAttempts = 5;
    public const string AllowedUserNameCharacters =
        "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
}

public static class CorsPolicyNames
{
    public const string All = nameof(All);
}

/// <summary>
/// Tags for API endpoints (used for Swagger documentation)
/// </summary>
public static class TagsConstants
{
    public const string Users = nameof(Users);
    public const string Mentors = nameof(Mentors);
    public const string Mentees = nameof(Mentees);
    public const string Notifications = nameof(Notifications);
    public const string Recommendations = nameof(Recommendations);
    public const string Ratings = nameof(Ratings);
    public const string Scheduling = nameof(Scheduling);
    public const string Materials = nameof(Materials);
}

/// <summary>
/// Schema names for the database
/// </summary>
public static class SchemaConstants
{
    public const string Users = "users";
    public const string Recommendations = "recommendations";
    public const string Ratings = "ratings";
    public const string Scheduling = "scheduling";
    public const string Materials = "materials";
}

/// <summary>
/// Policy names for authorization
/// </summary>
public static class PolicyConstants
{
    public const string ActiveUserOnly = nameof(ActiveUserOnly);
    public const string AdminOnly = nameof(AdminOnly);
    public const string MentorOnly = nameof(MentorOnly);
    public const string MenteeOnly = nameof(MenteeOnly);

    public const string AdminMentorMix = nameof(AdminMentorMix);
    public const string AdminMenteeMix = nameof(AdminMenteeMix);
    public const string MentorMenteeMix = nameof(MentorMenteeMix);
}

/// <summary>
/// Roles for users in the system
/// </summary>
public static class Roles
{
    public const string Admin = "Admin";
    public const string Mentor = "Mentor";
    public const string Mentee = "Mentee";
}

/// <summary>
/// Container names for Azure Blob Storage
/// </summary>
public static class ContainerNames
{
    public const string Avatars = "user-avatars";
    public const string MaterialsAttachments = "material-attachments";
}

public static class ProgrammingLanguages
{
    public static List<string> Values { get; } = [
        "JavaScript",
        "TypeScript",
        "Python",
        "Java",
        "C#",
        "C++",
        "C",
        "PHP",
        "Ruby",
        "Swift",
        "Kotlin",
        "Go",
        "Rust",
        "Dart",
        "Scala",
        "Elixir",
        "Perl",
        "Powelshell",
        "F#",
        "Shell Scripting",
        "HTML",
        "CSS",
        "SQL",
        "R",
        "MATLAB",
        "Lua",
        "Haskell",
        "Julia",
        "Objective-C",
        "Visual Basic",
        "Assembly",
        "Groovy",
        "Clojure",
        "Erlang",
        "COBOL",
        "Fortran",
        "Solidity",
        "Scheme",
    ];
}
