namespace MentorSync.SharedKernel;

public static class GeneralConstants
{
    public const string DatabaseName = "MentorSyncDb";
    public const string DefaultMigrationsTableName = "MigrationsHistory";
    public const string DefaultPassword = "qadbsfzYFJHS!";
    public const string DefaultEmail = "donotreply@7e133b28-71cb-4851-8bf4-701df3a6ce78.azurecomm.net";
}

public static class CorsPolicyNames
{
    public const string All = nameof(All);
}

public static class TagsConstants
{
    public const string Users = nameof(Users);
    public const string Notifications = nameof(Notifications);
}

public static class SchemaConstants
{
    public const string Users = "users";
}

public static class PolicyConstants
{
    public const string ActiveUserOnly = nameof(ActiveUserOnly);
    public const string AdminOnly = nameof(AdminOnly);
    public const string MentorOnly = nameof(MentorOnly);
    public const string MenteeOnly = nameof(MenteeOnly);
}

public static class Roles
{
    public const string Admin = "Admin";
    public const string Mentor = "Mentor";
    public const string Mentee = "Mentee";
}
