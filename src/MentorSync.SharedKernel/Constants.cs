namespace MentorSync.SharedKernel;

public static class GeneralConstants
{
    public const string DatabaseName = "MentorSyncDb";
    public const string DefaultMigrationsTableName = "MigrationsHistory";
    public const string DefaultPassword = "qadbsfzYFJHS!";
    public const string DefaultEmail = "donotreply@b434e76a-d5fa-4933-a565-aaeec6eee436.azurecomm.net";
}

public static class TagsConstants
{
    public const string Users = nameof(Users);
    public const string Google = "Google auth";
}

public static class SchemaConstants
{
    public const string Users = "users";
}

public static class Roles
{
    public const string Admin = "Admin";
    public const string Mentor = "Mentor";
    public const string Mentee = "Mentee";
}
