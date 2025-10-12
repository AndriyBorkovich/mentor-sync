using MentorSync.Ratings.Domain;
using Microsoft.EntityFrameworkCore;

namespace MentorSync.Ratings.Data;

/// <summary>
/// Database context for the Ratings module
/// </summary>
/// <param name="options">Context Options</param>
public sealed class RatingsDbContext(DbContextOptions<RatingsDbContext> options) : DbContext(options)
{
	/// <summary>
	/// Reviews for mentors
	/// </summary>
	public DbSet<MentorReview> MentorReviews { get; set; }
	/// <summary>
	/// Reviews for learning materials
	/// </summary>
	public DbSet<MaterialReview> MaterialReviews { get; set; }

	/// <inheritdoc />
	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.HasDefaultSchema(SchemaConstants.Ratings);

		const int reviewTextMaxLength = 2000;

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
				  .HasMaxLength(reviewTextMaxLength);
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

		modelBuilder.Entity<MaterialReview>(entity =>
		{
			entity.ToTable("MaterialReviews");
			entity.HasKey(e => e.Id);

			entity.Property(e => e.MaterialId)
				  .IsRequired();
			entity.Property(e => e.ReviewerId)
				  .IsRequired();
			entity.Property(e => e.Rating)
				  .IsRequired();
			entity.Property(e => e.ReviewText)
				  .HasMaxLength(reviewTextMaxLength);
			entity.Property(e => e.CreatedAt)
				.HasColumnType("timestamp with time zone")
				.HasDefaultValueSql("CURRENT_TIMESTAMP")
				.ValueGeneratedOnAdd()
				.IsRequired();
			entity.Property(e => e.UpdatedAt)
				.HasColumnType("timestamp with time zone")
				.HasDefaultValueSql("CURRENT_TIMESTAMP")
				.ValueGeneratedOnUpdate();

			entity.HasIndex(e => e.MaterialId);
			entity.HasIndex(e => e.ReviewerId);
		});
	}
}
