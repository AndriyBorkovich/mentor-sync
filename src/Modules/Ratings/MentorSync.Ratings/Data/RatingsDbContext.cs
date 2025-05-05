using MentorSync.Ratings.Domain;
using MentorSync.SharedKernel;
using Microsoft.EntityFrameworkCore;

namespace MentorSync.Ratings.Data;

public sealed class RatingsDbContext(DbContextOptions<RatingsDbContext> options) : DbContext(options)
{
    public DbSet<MentorReview> MentorReviews { get; set; }
    public DbSet<ArticleReview> ArticleReviews { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(SchemaConstants.Ratings);

        modelBuilder.Entity<MentorReview>(entity =>
        {
            entity.ToTable("MentorReviews");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.MentorId)
                  .IsRequired();
            entity.Property(e => e.MenteeId)
                  .IsRequired();
            entity.Property(e => e.Rating)
                  .IsRequired();
            entity.Property(e => e.ReviewText)
                  .HasMaxLength(2000);
            entity.Property(e => e.CreatedAt)
                .HasColumnType("timestamp with time zone")
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .ValueGeneratedOnAdd()
                .IsRequired();
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("timestamp with time zone")
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .ValueGeneratedOnUpdate();

            entity.HasIndex(e => e.MentorId);
            entity.HasIndex(e => e.MenteeId);
        });

        modelBuilder.Entity<ArticleReview>(entity =>
        {
            entity.ToTable("ArticleReviews");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.ArticleId)
                  .IsRequired();
            entity.Property(e => e.ReviewerId)
                  .IsRequired();
            entity.Property(e => e.Rating)
                  .IsRequired();
            entity.Property(e => e.ReviewText)
                  .HasMaxLength(2000);
            entity.Property(e => e.CreatedAt)
                .HasColumnType("timestamp with time zone")
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .ValueGeneratedOnAdd()
                .IsRequired();
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("timestamp with time zone")
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .ValueGeneratedOnUpdate();

            entity.HasIndex(e => e.ArticleId);
            entity.HasIndex(e => e.ReviewerId);
        });
    }
}
