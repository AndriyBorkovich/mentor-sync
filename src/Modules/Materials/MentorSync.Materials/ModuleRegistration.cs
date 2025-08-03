using MentorSync.Materials.Contracts.Services;
using MentorSync.Materials.Data;
using MentorSync.Materials.Services;
using MentorSync.SharedKernel;
using MentorSync.SharedKernel.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace MentorSync.Materials;

public static class ModuleRegistration
{
	public static void AddMaterialsModule(this IHostApplicationBuilder builder)
	{
		AddDatabase(builder);

		AddEndpoints(builder.Services);

		AddExternalServices(builder.Services);
	}

	private static void AddDatabase(IHostApplicationBuilder builder)
		=> builder.AddNpgsqlDbContext<MaterialsDbContext>(
				connectionName: GeneralConstants.DatabaseName,
				configureSettings: c => c.DisableTracing = true,
				configureDbContextOptions: opt =>
				{
					opt.UseNpgsql(b => b.MigrationsHistoryTable(GeneralConstants.DefaultMigrationsTableName, SchemaConstants.Materials));
				});

	private static void AddEndpoints(IServiceCollection services)
	{
		var assembly = typeof(ModuleRegistration).Assembly;
		services.AddValidators(assembly);
		services.AddHandlers(assembly);
		services.AddEndpoints(assembly);
	}

	private static void AddExternalServices(IServiceCollection services)
	{
		services.AddScoped<ILearningMaterialsService, LearningMaterialsService>();
	}
}
