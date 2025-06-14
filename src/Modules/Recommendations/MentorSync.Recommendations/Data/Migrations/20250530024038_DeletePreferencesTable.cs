using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MentorSync.Recommendations.Data.Migrations;

/// <inheritdoc />
public partial class DeletePreferencesTable : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "MenteePreferences",
            schema: "recommendations");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "MenteePreferences",
            schema: "recommendations",
            columns: table => new
            {
                Id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                DesiredIndustries = table.Column<int>(type: "integer", nullable: false),
                DesiredProgrammingLanguages = table.Column<List<string>>(type: "text[]", nullable: true),
                MenteeId = table.Column<int>(type: "integer", nullable: false),
                MinMentorExperienceYears = table.Column<int>(type: "integer", nullable: true),
                UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_MenteePreferences", x => x.Id);
            });
    }
}
