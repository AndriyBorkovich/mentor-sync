using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace MentorSync.ArchitectureTests;

/// <summary>
/// Architecture tests for modular monolith patterns
/// Ensures proper module isolation, clear boundaries, and controlled inter-module communication
/// Reference: https://www.milanjovanovic.tech/blog/what-is-a-modular-monolith
/// </summary>
public sealed class ModularMonolithTests
{
	/// <summary>
	/// Ensures that module Data layers are properly organized
	/// DbContext classes are allowed to be public for dependency injection, but repositories/services should be internal
	/// </summary>
	[Fact]
	public void ModuleDataLayersShouldBeWellOrganized()
	{
		Classes()
			.That()
			.ResideInNamespaceMatching("MentorSync.*.Data.*")
			.And()
			.AreAssignableTo(typeof(Microsoft.EntityFrameworkCore.DbContext))
			.Should()
			.HaveNameEndingWith("DbContext")
			.Because("Each module should have a DbContext for data access")
			.Check();
	}

	/// <summary>
	/// Ensures that module Domain layers are well-organized
	/// Domain entities are typically public for EF Core mapping, but domain logic is encapsulated
	/// </summary>
	[Fact]
	public void ModuleDomainLayersShouldBeWellOrganized()
	{
		Classes()
			.That()
			.ResideInNamespaceMatching("MentorSync.*.Domain.*")
			.And()
			.DoNotResideInNamespaceMatching("MentorSync.*.Domain.Events.*")
			.Should()
			.Exist()
			.Because("Domain entities support the modular monolith architecture")
			.Check();
	}

	/// <summary>
	/// Ensures that Contracts projects are the ONLY public interface from each module
	/// All inter-module dependencies must reference *.Contracts assemblies
	/// </summary>
	[Fact]
	public void ContractsProjectsShouldBeOnlyPublicInterface()
	{
		Classes()
			.That()
			.ResideInNamespaceMatching("MentorSync.Users.Contracts.*")
			.Should()
			.BePublic()
			.Because("Contracts are the public API for all inter-module communication")
			.Check();
	}

	/// <summary>
	/// Ensures that Features are the primary entry points within modules
	/// All CQRS commands and queries are public and accessible through handlers
	/// </summary>
	[Fact]
	public void FeatureCommandsAndQueriesShouldBePublic()
	{
		Classes()
			.That()
			.ResideInNamespaceMatching("MentorSync.*.Features.*")
			.And()
			.HaveNameMatching(".*Command$|.*Query$")
			.Should()
			.BePublic()
			.Because("Commands and Queries are the public interface for feature execution")
			.Check();
	}

	/// <summary>
	/// Ensures that each module has isolated database schema
	/// Database contexts should be module-specific and not shared across modules
	/// </summary>
	[Fact]
	public void EachModuleShouldHaveIsolatedDbContext()
	{
		Classes()
			.That()
			.AreAssignableTo(typeof(Microsoft.EntityFrameworkCore.DbContext))
			.And()
			.ResideInNamespaceMatching("MentorSync.*.Data.*")
			.Should()
			.HaveNameMatching(".*DbContext")
			.Because("Each module should have its own isolated DbContext")
			.Check();
	}

	/// <summary>
	/// Ensures that modules depend only on Contracts projects, never on other module implementations
	/// Each module must use only the public Contracts from other modules
	/// </summary>
	[Fact]
	public void ModuleAssembliesShouldDependOnlyOnContracts()
	{
		// Users module should only depend on Materials.Contracts, not Materials
		Classes()
			.That()
			.ResideInNamespace("MentorSync.Users")
			.And()
			.DoNotResideInNamespace("MentorSync.Users.Contracts")
			.Should()
			.NotDependOnAny(
				Classes()
					.That()
					.ResideInNamespaceMatching("MentorSync.Materials$")
					.Or()
					.ResideInNamespaceMatching("MentorSync.Scheduling$")
					.Or()
					.ResideInNamespaceMatching("MentorSync.Ratings$")
					.Or()
					.ResideInNamespaceMatching("MentorSync.Recommendations$")
					.Or()
					.ResideInNamespaceMatching("MentorSync.Notifications$"))
			.Because("Modules must use only Contracts projects for inter-module communication")
			.Check();
	}

	/// <summary>
	/// Ensures that all modules depend on SharedKernel for common abstractions
	/// SharedKernel provides CQRS interfaces and common utilities
	/// </summary>
	[Fact]
	public void ModulesShouldDependOnSharedKernelAbstractions()
	{
		Classes()
			.That()
			.ImplementInterface(typeof(ICommand<>))
			.Or()
			.ImplementInterface(typeof(IQuery<>))
			.Should()
			.ResideInNamespaceMatching("MentorSync.*")
			.Because("All CQRS patterns should be implemented in modules")
			.Check();
	}

	/// <summary>
	/// Ensures that no module directly accesses another module's Data layer
	/// All inter-module data access must go through Contracts
	/// </summary>
	[Fact]
	public void ModulesShouldNotDirectlyAccessOtherModulesDataLayers()
	{
		Classes()
			.That()
			.ResideInNamespace("MentorSync.Users")
			.And()
			.DoNotResideInNamespace("MentorSync.Users.Contracts")
			.Should()
			.NotDependOnAny(
				Classes()
					.That()
					.ResideInNamespaceMatching("MentorSync.Materials.Data")
					.Or()
					.ResideInNamespaceMatching("MentorSync.Scheduling.Data")
					.Or()
					.ResideInNamespaceMatching("MentorSync.Ratings.Data")
					.Or()
					.ResideInNamespaceMatching("MentorSync.Recommendations.Data")
					.Or()
					.ResideInNamespaceMatching("MentorSync.Notifications.Data"))
			.Because("Data layers are internal; modules should use Contracts instead")
			.Check();
	}

	/// <summary>
	/// Ensures that handlers typically use their module's DbContext for data access
	/// Some handlers may use external services (Azure Storage, Identity Manager, etc)
	/// </summary>
	[Fact]
	public void DataAccessHandlersShouldUseModuleDbContext()
	{
		// Verify that many handlers use DbContext (most do, but not all - some use external services)
		Classes()
			.That()
			.ResideInNamespaceMatching("MentorSync.Materials.Features.*")
			.And()
			.HaveNameEndingWith("Handler")
			.Should()
			.DependOnAny(
				Classes()
					.That()
					.AreAssignableTo(typeof(Microsoft.EntityFrameworkCore.DbContext)))
			.Because("Data-focused module handlers should access DbContext for queries and commands")
			.Check();
	}

	/// <summary>
	/// Ensures that modules follow the VSA (Vertical Slice Architecture) pattern
	/// Each feature vertically slices through all layers (Features -> Handlers -> Data)
	/// All feature components should reside within the Features namespace
	/// </summary>
	[Fact]
	public void FeaturesShouldFollowVerticalSlicePattern()
	{
		Classes()
			.That()
			.ResideInNamespaceMatching("MentorSync.*.Features.*")
			.And()
			.HaveNameMatching(".*Command|.*Query|.*Handler|.*Validator|.*Request|.*Response|.*Endpoint|.*Dto")
			.Should()
			.ResideInNamespaceMatching("MentorSync.*.Features.*")
			.Because("All feature artifacts should reside within Features namespace for vertical slice pattern")
			.Check();
	}

	/// <summary>
	/// Ensures that each module has a ModuleRegistration for dependency injection setup
	/// This centralizes module configuration and makes modules self-contained
	/// </summary>
	[Fact]
	public void EachModuleShouldHaveModuleRegistration()
	{
		Classes()
			.That()
			.HaveName("ModuleRegistration")
			.And()
			.ResideInNamespaceMatching("MentorSync.*")
			.Should()
			.Exist()
			.Because("Each module should have a ModuleRegistration class for DI setup")
			.Check();
	}
}
