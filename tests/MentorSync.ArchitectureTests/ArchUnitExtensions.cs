using ArchUnitNET.Domain;
using ArchUnitNET.Fluent;
using ArchUnitNET.Loader;
using ArchUnitNET.xUnitV3;
using MentorSync.API;

namespace MentorSync.ArchitectureTests;

/// <summary>
/// Extension methods and utilities for ArchUnitNET testing.
/// Loads the entire MentorSync architecture including all modules.
/// </summary>
internal static class ArchUnitExtensions
{
	private static readonly Lazy<Architecture> _architecture = new(() =>
	{
		var apiAssembly = typeof(GlobalExceptionHandler).Assembly;
		var sharedKernelAssembly = typeof(PaginatedList<>).Assembly;
		var migrationsAssembly = typeof(MigrationService.Worker).Assembly;
		var usersModuleAssembly = typeof(Users.Data.UsersDbContext).Assembly;
		var materialsModuleAssembly = typeof(Materials.Data.MaterialsDbContext).Assembly;
		var ratingsModuleAssembly = typeof(Ratings.Data.RatingsDbContext).Assembly;
		var schedulingModuleAssembly = typeof(Scheduling.Data.SchedulingDbContext).Assembly;
		var recommendationsModuleAssembly = typeof(Recommendations.Data.RecommendationsDbContext).Assembly;
		var notificationsModuleAssembly = typeof(Notifications.Data.NotificationsDbContext).Assembly;

		var usersContractsAssembly = typeof(Users.Contracts.Models.MenteePreferences).Assembly;
		var materialsContractsAssembly = typeof(Materials.Contracts.Models.LearningMaterialModel).Assembly;
		var ratingsContractsAssembly = typeof(Ratings.Contracts.Models.MaterialReviewResult).Assembly;
		var schedulingContractsAssembly = typeof(Scheduling.Contracts.Models.BookingModel).Assembly;
		var notificationsContractsAssembly = typeof(Notifications.Contracts.Models.SendEmailCommand).Assembly;

		var assembliesToLoad = new[]
			{
				apiAssembly,
				sharedKernelAssembly,
				migrationsAssembly,
				usersModuleAssembly,
				materialsModuleAssembly,
				ratingsModuleAssembly,
				schedulingModuleAssembly,
				recommendationsModuleAssembly,
				notificationsModuleAssembly,
				usersContractsAssembly,
				materialsContractsAssembly,
				ratingsContractsAssembly,
				schedulingContractsAssembly,
				notificationsContractsAssembly
			}
			.ToArray();

		var arch = new ArchLoader()
			.LoadAssemblies(assembliesToLoad)
			.Build();
		return arch;
	});

	/// <summary>
	/// Gets the loaded architecture for testing.
	/// Includes all modules and the core API assembly.
	/// </summary>
	private static Architecture architectureValue => _architecture.Value;

	public static void Check(this IArchRule rule) => rule.Check(architectureValue);
}
