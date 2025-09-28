namespace MentorSync.SharedKernel;

/// <summary>
/// General constants used throughout the application
/// </summary>
public static class GeneralConstants
{
	/// <summary>
	/// The name of the main database
	/// </summary>
	public const string DatabaseName = "MentorSyncDb";

	/// <summary>
	/// The default table name for Entity Framework migrations history
	/// </summary>
	public const string DefaultMigrationsTableName = "MigrationsHistory";

	/// <summary>
	/// Default password used for system operations
	/// </summary>
	public const string DefaultPassword = "qadbsfzYFJHS!";

	/// <summary>
	/// Default email address for system notifications
	/// </summary>
	public const string DefaultEmail = "donotreply@7e133b28-71cb-4851-8bf4-701df3a6ce78.azurecomm.net";

	/// <summary>
	/// Minimum required length for user passwords
	/// </summary>
	public const int MinPasswordLength = 8;

	/// <summary>
	/// Maximum allowed length for email addresses
	/// </summary>
	public const int MaxEmailLength = 256;

	/// <summary>
	/// Default lockout time in minutes for failed login attempts
	/// </summary>
	public const int DefaultLockoutTimeInMinutes = 5;

	/// <summary>
	/// Protection token validity time in hours
	/// </summary>
	public const int ProtectionTokenTimeInHours = 2;

	/// <summary>
	/// Maximum number of failed access attempts before lockout
	/// </summary>
	public const int MaxFailedAccessAttempts = 5;

	/// <summary>
	/// Characters allowed in usernames
	/// </summary>
	public const string AllowedUserNameCharacters =
		"abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+/ ";
}

/// <summary>
/// CORS policy names used in the application
/// </summary>
public static class CorsPolicyNames
{
	/// <summary>
	/// Policy name that allows all origins, methods, and headers
	/// </summary>
	public const string All = nameof(All);
}

/// <summary>
/// Tags for API endpoints (used for Swagger documentation)
/// </summary>
public static class TagsConstants
{
	/// <summary>
	/// Tag for user-related endpoints
	/// </summary>
	public const string Users = nameof(Users);

	/// <summary>
	/// Tag for mentor-related endpoints
	/// </summary>
	public const string Mentors = nameof(Mentors);

	/// <summary>
	/// Tag for mentee-related endpoints
	/// </summary>
	public const string Mentees = nameof(Mentees);

	/// <summary>
	/// Tag for notification-related endpoints
	/// </summary>
	public const string Notifications = nameof(Notifications);

	/// <summary>
	/// Tag for recommendation-related endpoints
	/// </summary>
	public const string Recommendations = nameof(Recommendations);

	/// <summary>
	/// Tag for rating-related endpoints
	/// </summary>
	public const string Ratings = nameof(Ratings);

	/// <summary>
	/// Tag for scheduling-related endpoints
	/// </summary>
	public const string Scheduling = nameof(Scheduling);

	/// <summary>
	/// Tag for learning material-related endpoints
	/// </summary>
	public const string Materials = nameof(Materials);
}

/// <summary>
/// Schema names for the database
/// </summary>
public static class SchemaConstants
{
	/// <summary>
	/// Schema name for user-related tables
	/// </summary>
	public const string Users = "users";

	/// <summary>
	/// Schema name for recommendation-related tables
	/// </summary>
	public const string Recommendations = "recommendations";

	/// <summary>
	/// Schema name for rating-related tables
	/// </summary>
	public const string Ratings = "ratings";

	/// <summary>
	/// Schema name for scheduling-related tables
	/// </summary>
	public const string Scheduling = "scheduling";

	/// <summary>
	/// Schema name for learning material-related tables
	/// </summary>
	public const string Materials = "materials";
}

/// <summary>
/// Policy names for authorization
/// </summary>
public static class PolicyConstants
{
	/// <summary>
	/// Policy requiring active user status
	/// </summary>
	public const string ActiveUserOnly = nameof(ActiveUserOnly);

	/// <summary>
	/// Policy requiring admin role
	/// </summary>
	public const string AdminOnly = nameof(AdminOnly);

	/// <summary>
	/// Policy requiring mentor role
	/// </summary>
	public const string MentorOnly = nameof(MentorOnly);

	/// <summary>
	/// Policy requiring mentee role
	/// </summary>
	public const string MenteeOnly = nameof(MenteeOnly);

	/// <summary>
	/// Policy allowing admin or mentor roles
	/// </summary>
	public const string AdminMentorMix = nameof(AdminMentorMix);

	/// <summary>
	/// Policy allowing admin or mentee roles
	/// </summary>
	public const string AdminMenteeMix = nameof(AdminMenteeMix);

	/// <summary>
	/// Policy allowing mentor or mentee roles
	/// </summary>
	public const string MentorMenteeMix = nameof(MentorMenteeMix);
}

/// <summary>
/// Roles for users in the system
/// </summary>
public static class Roles
{
	/// <summary>
	/// Administrator role with full system access
	/// </summary>
	public const string Admin = "Admin";

	/// <summary>
	/// Mentor role for users providing guidance
	/// </summary>
	public const string Mentor = "Mentor";

	/// <summary>
	/// Mentee role for users seeking guidance
	/// </summary>
	public const string Mentee = "Mentee";
}

/// <summary>
/// Container names for Azure Blob Storage
/// </summary>
public static class ContainerNames
{
	/// <summary>
	/// Container for storing user avatar images
	/// </summary>
	public const string Avatars = "user-avatars";

	/// <summary>
	/// Container for storing learning material attachments
	/// </summary>
	public const string MaterialsAttachments = "material-attachments";
}

/// <summary>
/// Supported programming languages in the mentor-sync system
/// </summary>
public static class ProgrammingLanguages
{
	/// <summary>
	/// Gets a read-only list of all supported programming languages
	/// </summary>
	/// <example>
	/// <code>
	/// var languages = ProgrammingLanguages.Values;
	/// var hasJavaScript = languages.Contains("JavaScript"); // true
	/// </code>
	/// </example>
	public static IReadOnlyList<string> Values { get; } = [
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
