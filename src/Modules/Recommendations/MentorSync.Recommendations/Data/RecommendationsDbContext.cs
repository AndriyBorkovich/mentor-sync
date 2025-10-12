using MentorSync.Recommendations.Domain.Interaction;
using MentorSync.Recommendations.Domain.Result;
using MentorSync.Recommendations.Domain.Tracking;
using Microsoft.EntityFrameworkCore;

namespace MentorSync.Recommendations.Data;

/// <inheritdoc />
public sealed class RecommendationsDbContext(DbContextOptions<RecommendationsDbContext> options)
	: DbContext(options)
{
	/// <summary>
	/// Mentee-Mentor interaction records
	/// </summary>
	public DbSet<MentorMenteeInteraction> MenteeMentorInteractions { get; set; }
	/// <summary>
	/// Mentor view events
	/// </summary>
	public DbSet<MentorViewEvent> MentorViewEvents { get; set; }
	/// <summary>
	/// Mentor bookmark events
	/// </summary>
	public DbSet<MentorBookmark> MentorBookmarks { get; set; }
	/// <summary>
	/// Mentor review results
	/// </summary>
	public DbSet<MentorRecommendationResult> MentorRecommendationResults { get; set; }

	/// <summary>
	/// Mentee-Material interaction records
	/// </summary>
	public DbSet<MenteeMaterialInteraction> MenteeMaterialInteractions { get; set; }
	/// <summary>
	/// Material view events
	/// </summary>
	public DbSet<MaterialViewEvent> MaterialViewEvents { get; set; }
	/// <summary>
	/// Material like events
	/// </summary>
	public DbSet<MaterialLike> MaterialLikes { get; set; }
	/// <summary>
	/// Material review results
	/// </summary>
	public DbSet<MaterialRecommendationResult> MaterialRecommendationResults { get; set; }

	/// <inheritdoc />
	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);

		modelBuilder.HasDefaultSchema(SchemaConstants.Recommendations);
	}
}
