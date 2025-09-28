using System.Collections.Immutable;
using Bogus;
using MentorSync.Materials.Data;
using MentorSync.Materials.Domain;
using MentorSync.Ratings.Data;
using MentorSync.Ratings.Domain;
using MentorSync.Recommendations.Data;
using MentorSync.Recommendations.Domain.Tracking;
using MentorSync.Users.Data;
using Microsoft.EntityFrameworkCore;

namespace MentorSync.MigrationService.Seeders;

public static class LearningMaterialsSeeder
{
	private static readonly ImmutableList<(string Title, string Description, string[] Tags)> _programmingTopics =
		ImmutableList.Create(
			("Understanding SOLID Principles", "A comprehensive guide to SOLID principles in modern software development",
				["design-patterns", "best-practices", "architecture"]),
			("Mastering Git Workflow", "Learn professional Git workflow patterns and best practices",
				["git", "version-control", "collaboration"]),
			("Clean Code: Best Practices", "Writing clean, maintainable, and efficient code",
				["clean-code", "best-practices", "code-quality"]),
			("Design Patterns in C#", "Implementing common design patterns in C# applications",
				["csharp", "design-patterns", "object-oriented"]),
			("RESTful API Design", "Best practices for designing RESTful APIs",
				["api", "web-development", "rest"]),
			("Microservices Architecture", "Introduction to microservices architecture and patterns",
				["microservices", "architecture", "distributed-systems"]),
			("Unit Testing Fundamentals", "Writing effective unit tests and following TDD",
				["testing", "tdd", "best-practices"]),
			("Dependency Injection Explained", "Understanding DI patterns and implementations",
				["dependency-injection", "design-patterns", "architecture"]),
			("Async Programming in C#", "Mastering asynchronous programming patterns",
				["csharp", "async", "performance"]),
			("Domain-Driven Design", "Implementing DDD principles in modern applications",
				["ddd", "architecture", "design-patterns"]),
			("Entity Framework Core Best Practices", "Optimizing database operations with EF Core",
				["entity-framework", "database", "performance"]),
			("Security Best Practices", "Implementing security in modern applications",
				["security", "best-practices", "authentication"]),
			("CI/CD Pipeline Implementation", "Setting up robust CI/CD pipelines",
				["devops", "ci-cd", "automation"]),
			("Docker Containerization", "Getting started with Docker and containers",
				["docker", "devops", "containers"]),
			("Kubernetes for Developers", "Understanding Kubernetes fundamentals",
				["kubernetes", "devops", "containers"]),
			("Event-Driven Architecture", "Building event-driven microservices",
				["architecture", "events", "microservices"]),
			("GraphQL API Development", "Building efficient GraphQL APIs",
				["graphql", "api", "web-development"]),
			("Redis Caching Strategies", "Implementing effective caching with Redis",
				["redis", "caching", "performance"]),
			("MongoDB Best Practices", "Working effectively with MongoDB",
				["mongodb", "database", "nosql"]),
			("Azure Cloud Architecture", "Designing solutions for Azure cloud",
				["azure", "cloud", "architecture"]),
			("Blazor Web Development", "Building modern web apps with Blazor",
				["blazor", "web-development", "csharp"]),
			("gRPC Service Development", "Implementing efficient gRPC services",
				["grpc", "microservices", "performance"]),
			("Message Queue Patterns", "Working with RabbitMQ and message queues",
				["messaging", "rabbitmq", "architecture"]),
			("OAuth2 Implementation", "Securing applications with OAuth2",
				["security", "oauth", "authentication"]),
			("Elasticsearch in .NET", "Implementing search with Elasticsearch",
				["elasticsearch", "search", "performance"]),
			("Machine Learning Basics", "Introduction to ML concepts and implementation",
				["machine-learning", "ai", "data-science"]),
			("Code Review Best Practices", "Conducting effective code reviews",
				["code-review", "collaboration", "best-practices"]),
			("Performance Optimization", "Optimizing .NET application performance",
				["performance", "optimization", "best-practices"]),
			("Logging and Monitoring", "Setting up proper logging and monitoring",
				["logging", "monitoring", "devops"]),
			("API Security Patterns", "Securing APIs and web services",
				new[] { "security", "api", "authentication" })
		);

	public static async Task SeedAsync(IServiceProvider serviceProvider, ILogger logger)
	{
		await using var scope = serviceProvider.CreateAsyncScope();
		var materialsContext = scope.ServiceProvider.GetRequiredService<MaterialsDbContext>();
		var usersContext = scope.ServiceProvider.GetRequiredService<UsersDbContext>();
		var ratingsContext = scope.ServiceProvider.GetRequiredService<RatingsDbContext>();
		var recommendationsContext = scope.ServiceProvider.GetRequiredService<RecommendationsDbContext>();

		if (await materialsContext.LearningMaterials.AnyAsync())
		{
			logger.LogInformation("Learning materials already seeded. Skipping seeding process.");
			return;
		}

		// Get mentors and mentees for seeding
		var mentorIds = await usersContext.MentorProfiles.Select(m => m.MentorId).ToListAsync();
		var menteeIds = await usersContext.MenteeProfiles.Select(m => m.MenteeId).ToListAsync();

		if (mentorIds.Count == 0 || menteeIds.Count == 0)
		{
			logger.LogWarning("No mentors or mentees found for seeding materials");
			return;
		}

		// Initialize Bogus faker
		var faker = new Faker { Random = new Randomizer(42) }; // Fixed seed for reproducibility

		// Generate learning materials from predefined topics
		var materials = new List<LearningMaterial>();
		var topics = _programmingTopics.ToList();

		materials.AddRange(from topic in topics
						   let createdAt = faker.Date.Recent(90).ToUniversalTime()
						   let material = new LearningMaterial
						   {
							   Title = topic.Title,
							   Description = topic.Description,
							   Type = MaterialType.Article,
							   ContentMarkdown = GenerateMarkdownContent(faker, topic.Title, topic.Description),
							   MentorId = faker.PickRandom(mentorIds),
							   CreatedAt = createdAt,
							   UpdatedAt = faker.Random.Bool(0.5f) ? faker.Date.Between(createdAt, DateTime.UtcNow).ToUniversalTime() : null,
							   Attachments = [],
							   Tags = [.. topic.Tags.Select(tagName => new Tag { Name = tagName })],
						   }
						   select material);
		await materialsContext.LearningMaterials.AddRangeAsync(materials);
		await materialsContext.SaveChangesAsync();

		// Generate view events (60-80% of mentees view each material)
		var viewEvents = materials.SelectMany(material =>
		{
			var viewerCount = (int)(menteeIds.Count * faker.Random.Double(0.6, 0.8));
			return faker.PickRandom(menteeIds, viewerCount)
				.Select(menteeId => new MaterialViewEvent
				{
					MaterialId = material.Id,
					MenteeId = menteeId,
					ViewedAt = faker.Date.Between(material.CreatedAt, DateTime.UtcNow).ToUniversalTime()
				});
		}).ToList();

		await recommendationsContext.MaterialViewEvents.AddRangeAsync(viewEvents);
		await recommendationsContext.SaveChangesAsync();

		// Generate reviews (20-40% of viewers leave a review)
		var reviews = materials.SelectMany(material =>
		{
			var viewers = viewEvents.Where(v => v.MaterialId == material.Id).Select(v => v.MenteeId).ToList();
			var reviewerCount = (int)(viewers.Count * faker.Random.Double(0.2, 0.4));
			return faker.PickRandom(viewers, reviewerCount)
				.Select(menteeId =>
				{
					var rating = faker.Random.Int(3, 5);
					return new MaterialReview
					{
						MaterialId = material.Id,
						ReviewerId = menteeId,
						Rating = rating,
						ReviewText = GenerateReviewComment(faker, rating),
						CreatedAt = faker.Date.Between(
							viewEvents.First(v => v.MaterialId == material.Id && v.MenteeId == menteeId).ViewedAt,
							DateTime.UtcNow).ToUniversalTime(),
					};
				});
		}).ToList();

		await ratingsContext.MaterialReviews.AddRangeAsync(reviews);
		await ratingsContext.SaveChangesAsync();

		// Generate likes (40-60% of viewers like the material)
		var likes = materials.SelectMany(material =>
		{
			var viewers = viewEvents.Where(v => v.MaterialId == material.Id).Select(v => v.MenteeId).ToList();
			var likerCount = (int)(viewers.Count * faker.Random.Double(0.4, 0.6));
			return faker.PickRandom(viewers, likerCount)
				.Select(menteeId => new MaterialLike
				{
					MaterialId = material.Id,
					MenteeId = menteeId,
					LikedAt = faker.Date.Between(
						viewEvents.First(v => v.MaterialId == material.Id && v.MenteeId == menteeId).ViewedAt,
						DateTime.UtcNow).ToUniversalTime()
				});
		}).ToList();

		await recommendationsContext.MaterialLikes.AddRangeAsync(likes);
		await recommendationsContext.SaveChangesAsync();

		logger.LogInformation(
			"Seeded {MaterialCount} materials with {ViewCount} views, {ReviewCount} reviews, and {LikeCount} likes",
			materials.Count,
			viewEvents.Count,
			reviews.Count,
			likes.Count);
	}

	private static string GenerateMarkdownContent(Faker faker, string title, string description)
	{
		var keyPoints = new[]
		{
			"Understanding core principles and patterns",
			"Following industry best practices",
			"Avoiding common pitfalls and mistakes",
			"Performance considerations and optimizations",
			"Security implications and guidelines",
			"Testing strategies and methodologies",
			"DevOps and deployment considerations",
			"Scalability and maintenance aspects"
		};

		var bestPractices = new[]
		{
			"Follow established design patterns",
			"Write clean and maintainable code",
			"Implement proper error handling",
			"Use dependency injection",
			"Write comprehensive unit tests",
			"Document your code thoroughly",
			"Consider security implications",
			"Optimize for performance",
			"Follow SOLID principles",
			"Use meaningful naming conventions"
		};

		return $@"# {title}

## Introduction

{description}. This comprehensive guide will help you understand and implement these concepts effectively in your projects.

### Key Points

{string.Join("\n", faker.PickRandom(keyPoints, faker.Random.Int(4, 6)).Select(point => $"- {point}"))}

## Main Content

{faker.Lorem.Paragraphs(3)}

### Real-World Scenarios

{faker.Lorem.Paragraph()}

### Code Example

```csharp
public class {faker.Hacker.Noun()}Service
{{
    private readonly I{faker.Hacker.Noun()}Repository _{faker.Hacker.Verb()}Repository;

    public async Task<Result> {faker.Hacker.Verb()}Async({faker.Hacker.Noun()} request)
    {{
        // Implementation details
        var result = await _{faker.Hacker.Verb()}Repository.{faker.Hacker.Verb()}Async(request);
        return Result.Success(result);
    }}
}}
```

## Best Practices

{string.Join('\n', faker.PickRandom(bestPractices, faker.Random.Int(4, 6)).Select((practice, i) => $"{i + 1}. {practice}"))}

## Implementation Considerations

{faker.Lorem.Paragraph()}

## Common Pitfalls

{faker.Lorem.Paragraph()}

## Conclusion

Thank you for reading this guide about {title}. Apply these concepts in your projects to write better, more maintainable code.

---
*Дата останнього оновлення: {faker.Date.Recent():MMMM d, yyyy}*";
	}
	private static string GenerateReviewComment(Faker faker, int rating)
	{
		// Ukrainian positive points for material reviews
		var positivePoints = new[]
		{
			"Дуже інформативний та добре структурований матеріал",
			"Чудові практичні приклади",
			"Зрозумілі пояснення складних концепцій",
			"Відмінні зразки коду",
			"Всебічне висвітлення теми",
			"Дуже практичний і застосовний контент",
			"Добре організований зміст",
			"Матеріал легко засвоюється",
			"Дуже корисно для професійного розвитку",
		};

		// Ukrainian suggestions for improvement
		var improvements = new[]
		{
			"можна додати більше практичних прикладів",
			"було б корисно мати детальніші пояснення",
			"деякі концепції потребують більше контексту",
			"варто включити більше реальних сценаріїв",
			"хотілося б бачити більше прикладів коду",
			"можна додати більше візуальних матеріалів",
			"деякі частини потребують спрощення",
		};

		// Ukrainian negative comments for low ratings
		var negativePoints = new[]
		{
			"Зміст занадто поверхневий",
			"Приклади застарілі",
			"Важко зрозуміти пояснення",
			"Не вистачає глибини",
			"Матеріал занадто теоретичний"
		};

		return rating switch
		{
			5 => $"{faker.PickRandom(positivePoints)}! {faker.PickRandom(positivePoints).ToLower()}. Дуже рекомендую!",
			4 => $"{faker.PickRandom(positivePoints)}. Але {faker.PickRandom(improvements).ToLower()}.",
			3 => $"Непоганий матеріал, але {faker.PickRandom(improvements).ToLower()}. Також {faker.PickRandom(improvements).ToLower()}.",
			2 => $"{faker.PickRandom(negativePoints)}. Потрібно доопрацювати.",
			_ => $"{faker.PickRandom(negativePoints)}. Не рекомендую.",
		};
	}
}
