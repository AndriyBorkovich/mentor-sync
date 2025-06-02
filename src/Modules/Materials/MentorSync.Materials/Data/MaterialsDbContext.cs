using MentorSync.Materials.Domain;
using MentorSync.SharedKernel;
using Microsoft.EntityFrameworkCore;

namespace MentorSync.Materials.Data;

public sealed class MaterialsDbContext(DbContextOptions<MaterialsDbContext> options) : DbContext(options)
{
    public DbSet<LearningMaterial> LearningMaterials { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public DbSet<MaterialAttachment> MaterialAttachments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(SchemaConstants.Materials);

        modelBuilder.Entity<LearningMaterial>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Title)
                  .IsRequired()
                  .HasMaxLength(200);

            entity.Property(e => e.Description)
                  .HasMaxLength(2000);

            entity.Property(e => e.ContentMarkdown)
                  .HasColumnType("text");

            entity.Property(e => e.Type)
                  .IsRequired();

            entity.Property(e => e.MentorId)
                  .IsRequired();

            entity.Property(e => e.CreatedAt)
                  .HasDefaultValueSql("CURRENT_TIMESTAMP")
                  .ValueGeneratedOnAdd();

            entity.Property(e => e.UpdatedAt)
                  .HasDefaultValueSql("CURRENT_TIMESTAMP")
                  .ValueGeneratedOnUpdate();

            entity.HasIndex(e => e.Title);
        });

        modelBuilder.Entity<Tag>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name)
                  .IsRequired()
                  .HasMaxLength(25);

            entity.Property(e => e.Description)
                  .HasMaxLength(100);

            entity.HasIndex(e => e.Name);
        });

        modelBuilder.Entity<MaterialAttachment>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.MaterialId)
                  .IsRequired();

            entity.Property(e => e.FileName)
                  .IsRequired()
                  .HasMaxLength(255);

            entity.Property(e => e.BlobUri)
                  .IsRequired()
                  .HasMaxLength(2000);

            entity.Property(e => e.ContentType)
                  .HasMaxLength(100);

            entity.Property(e => e.UploadedAt)
                  .HasDefaultValueSql("CURRENT_TIMESTAMP")
                  .ValueGeneratedOnAdd();
        });
    }
}

