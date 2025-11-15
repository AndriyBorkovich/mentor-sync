using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace MentorSync.ArchitectureTests;

/// <summary>
/// Architecture tests for layer dependencies and encapsulation
/// Ensures proper isolation between different layers of the application
/// </summary>
public sealed class LayerArchitectureTests
{
	/// <summary>
	/// Ensures that SharedKernel abstractions are public for all modules to use
	/// </summary>
	[Fact]
	public void SharedKernelAbstractionsShouldBePublic()
	{
		Interfaces()
			.That()
			.ResideInNamespaceMatching("MentorSync.SharedKernel.Abstractions.*")
			.Should()
			.BePublic()
			.Check();
	}

	/// <summary>
	/// Ensures that handlers only live in Features namespace
	/// Handlers are implementation details
	/// </summary>
	[Fact]
	public void HandlersShouldOnlyResideInFeatures()
	{
		Classes()
			.That()
			.ImplementAnyInterfaces(
				typeof(ICommandHandler<,>),
				typeof(IQueryHandler<,>)
			)
			.Should()
			.ResideInNamespaceMatching("MentorSync.*.Features.*")
			.Check();
	}

	/// <summary>
	/// Ensures DbContext is only in Data namespace and named appropriately
	/// </summary>
	[Fact]
	public void DbContextShouldFollowDataLayerPattern()
	{
		Classes()
			.That()
			.HaveNameEndingWith("DbContext")
			.Should()
			.ResideInNamespaceMatching("MentorSync.*.Data")
			.Check();
	}
}
