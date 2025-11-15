using FluentValidation;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace MentorSync.ArchitectureTests;

/// <summary>
/// Architecture tests for naming conventions across the application
/// Ensures consistent naming patterns for commands, queries, handlers, endpoints, and other types
/// </summary>
public sealed class NamingConventionTests
{
	/// <summary>
	/// Ensures that all method names start with a capital letter
	/// </summary>
	[Fact]
	public void AllMethodsShouldStartWithACapitalLetter()
		=> MethodMembers()
			   .That()
			   .AreNoConstructors()
			   .And()
			   .AreNotPrivate()
			   .And()
			   .DoNotHaveAnyAttributes(Attributes().That().HaveName("SpecialName").Or().HaveName("CompilerGenerated"))
			   .And()
			   .DoNotHaveNameMatching("^get_|^set_|^add_|^remove_|^op_") // Ignore property/event accessors and operator overloads compiler-generated names
			   .Should()
			   .HaveNameMatching("^[A-Z]")
			   .Because("C# convention...")
			   .Check();

	/// <summary>
	/// Ensures that command classes follow the naming convention of {Feature}Command
	/// </summary>
	[Fact]
	public void CommandsShouldFollowNamingConvention()
	{
		Classes()
			.That()
			.ImplementInterface(typeof(ICommand<>))
			.And()
			.ResideInNamespaceMatching("MentorSync.*")
			.Should()
			.HaveNameEndingWith("Command")
			.Check();
	}

	/// <summary>
	/// Ensures that command handlers follow the naming convention of {Feature}CommandHandler
	/// </summary>
	[Fact]
	public void CommandHandlersShouldFollowNamingConvention()
	{
		Classes()
			.That()
			.ImplementInterface(typeof(ICommandHandler<,>))
			.Should()
			.HaveNameEndingWith("CommandHandler")
			.Check();
	}

	/// <summary>
	/// Ensures that query classes follow the naming convention of {Feature}Query
	/// </summary>
	[Fact]
	public void QueriesShouldFollowNamingConvention()
	{
		Types()
			.That()
			.ImplementInterface(typeof(IQuery<>))
			.Should()
			.HaveNameEndingWith("Query")
			.Check();
	}

	/// <summary>
	/// Ensures that query handlers follow the naming convention of {Feature}QueryHandler
	/// </summary>
	[Fact]
	public void QueryHandlersShouldFollowNamingConvention()
	{
		Classes()
			.That()
			.ImplementInterface(typeof(IQueryHandler<,>))
			.Should()
			.HaveNameEndingWith("QueryHandler")
			.Check();
	}

	/// <summary>
	/// Ensures that endpoint classes follow the naming convention of {Feature}Endpoint
	/// </summary>
	[Fact]
	public void EndpointsShouldFollowNamingConvention()
	{
		Classes()
			.That()
			.ImplementInterface(typeof(IEndpoint))
			.Should()
			.HaveNameEndingWith("Endpoint")
			.Check();
	}

	/// <summary>
	/// Ensures that validator classes follow the naming convention of {Feature}Validator
	/// </summary>
	[Fact]
	public void ValidatorsShouldFollowNamingConvention()
	{
		Classes()
			.That()
			.ImplementInterface(typeof(IValidator<>))
			.Should()
			.HaveNameEndingWith("Validator")
			.Check();
	}

	/// <summary>
	/// Ensures that DbContext classes follow the naming convention of {Domain}DbContext
	/// </summary>
	[Fact]
	public void DbContextsShouldFollowNamingConvention()
	{
		Classes()
			.That()
			.AreAssignableTo(typeof(Microsoft.EntityFrameworkCore.DbContext))
			.Should()
			.HaveNameEndingWith("DbContext")
			.Check();
	}

	/// <summary>
	/// Ensures that interfaces follow the naming convention of prefixing with 'I'
	/// </summary>
	[Fact]
	public void InterfacesShouldFollowNamingConvention()
	{
		Interfaces()
			.That()
			.ResideInNamespaceMatching("MentorSync.*")
			.Should()
			.HaveNameStartingWith("I")
			.Check();
	}

	/// <summary>
	/// Ensures that Request DTOs reside in Features namespace
	/// </summary>
	[Fact]
	public void RequestDtosShouldResideInFeaturesNamespace()
	{
		Types()
			.That()
			.HaveNameEndingWith("Request")
			.And()
			.ResideInNamespaceMatching("MentorSync.*")
			.Should()
			.ResideInNamespaceMatching("MentorSync.*.Features.*")
			.Check();
	}

	/// <summary>
	/// Ensures that Response DTOs reside in Features and have consistent naming
	/// </summary>
	[Fact]
	public void ResponseDtosShouldFollowNamingConvention()
	{
		Types()
			.That()
			.HaveNameEndingWith("Response")
			.And()
			.ResideInNamespaceMatching("MentorSync.*.Features.*")
			.Should()
			.HaveNameEndingWith("Response")
			.Check();
	}
}
