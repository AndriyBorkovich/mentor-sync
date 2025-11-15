using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace MentorSync.ArchitectureTests;

/// <summary>
/// Architecture tests for CQRS and VSA patterns compliance
/// </summary>
public sealed class CqrsAndVsaPatternsTests
{
	/// <summary>
	/// Ensures that query handlers have name *QueryHandler and reside in Features namespace
	/// </summary>
	[Fact]
	public void QueryHandlersShouldFollowNamingAndLocationConvention()
	{
		Types()
			.That()
			.ImplementInterface(typeof(IQueryHandler<,>))
			.Should()
			.HaveNameEndingWith("QueryHandler")
			.AndShould()
			.ResideInNamespaceMatching("MentorSync.*.Features.*")
			.Check();
	}

	/// <summary>
	/// Ensures that commands reside in Features namespace
	/// </summary>
	[Fact]
	public void CommandsShouldResideInFeaturesNamespace()
	{
		var commands = Types()
			.That()
			.HaveNameEndingWith("Command")
			.And()
			.DoNotResideInAssemblyMatching("MentorSync.SharedKernel.*")
			.And()
			.DoNotResideInAssemblyMatching("MentorSync.Notifications.Contracts.*");

		commands
			.Should()
			.ResideInNamespaceMatching("MentorSync.*.Features.*")
			.Check();
	}

	/// <summary>
	/// Ensures that queries reside in Features namespace
	/// </summary>
	[Fact]
	public void QueriesShouldResideInFeaturesNamespace()
	{
		var queries = Types()
			.That()
			.HaveNameEndingWith("Query");

		queries
			.Should()
			.ResideInNamespaceMatching("MentorSync.*.Features.*")
			.Check();
	}

	/// <summary>
	/// Ensures that endpoints are only in Features namespace
	/// </summary>
	[Fact]
	public void EndpointsShouldResideInFeaturesNamespace()
	{
		Types()
			.That()
			.HaveNameEndingWith("Endpoint")
			.And()
			.ImplementInterface(typeof(IEndpoint))
			.Should()
			.ResideInNamespaceMatching("MentorSync.*.Features.*")
			.Check();
	}

	/// <summary>
	/// Ensures that validators reside in Features namespace
	/// </summary>
	[Fact]
	public void ValidatorsShouldResideInFeaturesNamespace()
	{
		Types()
			.That()
			.HaveNameEndingWith("Validator")
			.Should()
			.ResideInNamespaceMatching("MentorSync.*.Features.*")
			.Check();
	}

	/// <summary>
	/// Ensures that request/response DTOs reside in Features namespace
	/// </summary>
	[Fact]
	public void DtosShouldResideInFeaturesNamespace()
	{
		var dtos = Types()
			.That()
			.HaveNameEndingWith("Request")
			.Or()
			.HaveNameEndingWith("Response");

		dtos
		   .Should()
		   .ResideInNamespaceMatching("MentorSync.*.Features.*")
		   .Check();
	}
}
