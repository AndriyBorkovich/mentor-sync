using MentorSync.Recommendations.Domain.Interaction;
using MentorSync.Recommendations.Domain.Result;
using MentorSync.Recommendations.Domain.Tracking;
using MentorSync.SharedKernel;
using Microsoft.EntityFrameworkCore;

namespace MentorSync.Recommendations.Data;

public sealed class RecommendationsDbContext(DbContextOptions<RecommendationsDbContext> options)
    : DbContext(options)
{
    // Mentor recommendation entities
    public DbSet<MentorMenteeInteraction> MenteeMentorInteractions { get; set; }
    public DbSet<MentorViewEvent> MentorViewEvents { get; set; }
    public DbSet<MentorBookmark> MentorBookmarks { get; set; }
    public DbSet<MentorRecommendationResult> MentorRecommendationResults { get; set; }

    // Learning material recommendation entities
    public DbSet<MenteeMaterialInteraction> MenteeMaterialInteractions { get; set; }
    public DbSet<MaterialViewEvent> MaterialViewEvents { get; set; }
    public DbSet<MaterialLike> MaterialLikes { get; set; }
    public DbSet<MaterialRecommendationResult> MaterialRecommendationResults { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.HasDefaultSchema(SchemaConstants.Recommendations);
    }
}
