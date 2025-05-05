using MentorSync.Recommendations.Domain;
using MentorSync.Recommendations.Domain.Interaction;
using MentorSync.Recommendations.Domain.Preferences;
using MentorSync.Recommendations.Domain.Tracking;
using MentorSync.SharedKernel;
using Microsoft.EntityFrameworkCore;

namespace MentorSync.Recommendations.Data;

public sealed class RecommendationDbContext(DbContextOptions<RecommendationDbContext> options)
    : DbContext(options)
{
    public DbSet<MentorMenteeInteraction> MenteeMentorInteractions { get; set; }
    public DbSet<MenteePreference> MenteePreferences { get; set; }
    public DbSet<MentorViewEvent> MentorViewEvents { get; set; }
    public DbSet<MentorBookmark> MentorBookmarks { get; set; }
    public DbSet<RecommendationResult> RecommendationResults { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.HasDefaultSchema(SchemaConstants.Recommendations);
    }
}
