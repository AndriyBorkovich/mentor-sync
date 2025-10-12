using MentorSync.Scheduling.Domain;
using Microsoft.EntityFrameworkCore;

namespace MentorSync.Scheduling.Data;

/// <inheritdoc />
public sealed class SchedulingDbContext(DbContextOptions<SchedulingDbContext> options) : DbContext(options)
{
	/// <summary>
	/// Mentor availabilities
	/// </summary>
	public DbSet<MentorAvailability> MentorAvailabilities { get; set; }
	/// <summary>
	/// Bookings
	/// </summary>
	public DbSet<Booking> Bookings { get; set; }

	/// <inheritdoc />
	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.HasDefaultSchema(SchemaConstants.Scheduling);

		modelBuilder.Entity<MentorAvailability>(entity =>
		{
			entity.HasKey(e => e.Id);

			entity.Property(e => e.MentorId)
				  .IsRequired();
			entity.Property(e => e.Start)
				  .HasColumnType("timestamp with time zone")
				  .IsRequired();
			entity.Property(e => e.End)
				  .HasColumnType("timestamp with time zone")
				  .IsRequired();

			entity.HasIndex(e => e.MentorId);
		});

		modelBuilder.Entity<Booking>(entity =>
		{
			entity.HasKey(e => e.Id);

			entity.Property(e => e.MentorId)
				  .IsRequired();
			entity.Property(e => e.MenteeId)
				  .IsRequired();
			entity.Property(e => e.Start)
				  .HasColumnType("timestamp with time zone")
				  .IsRequired();
			entity.Property(e => e.End)
				  .HasColumnType("timestamp with time zone")
				  .IsRequired();
			entity.Property(e => e.Status)
				  .HasConversion<string>()
				  .HasMaxLength(20)
				  .IsRequired();

			entity.Property(e => e.CreatedAt)
				  .HasColumnType("timestamp with time zone")
				  .HasDefaultValueSql("CURRENT_TIMESTAMP")
				  .ValueGeneratedOnAdd();
			entity.Property(e => e.UpdatedAt)
				  .HasColumnType("timestamp with time zone")
				  .HasDefaultValueSql("CURRENT_TIMESTAMP")
				  .ValueGeneratedOnUpdate();

			entity.HasIndex(e => e.MentorId);
			entity.HasIndex(e => e.MenteeId);
		});
	}
}
