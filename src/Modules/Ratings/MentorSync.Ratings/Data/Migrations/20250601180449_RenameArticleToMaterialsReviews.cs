using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MentorSync.Ratings.Data.Migrations;

/// <inheritdoc />
public partial class RenameArticleToMaterialsReviews : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "ArticleReviews",
            schema: "ratings");

        migrationBuilder.CreateTable(
            name: "MaterialReviews",
            schema: "ratings",
            columns: table => new
            {
                Id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                MaterialId = table.Column<int>(type: "integer", nullable: false),
                ReviewerId = table.Column<int>(type: "integer", nullable: false),
                Rating = table.Column<int>(type: "integer", nullable: false),
                ReviewText = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_MaterialReviews", x => x.Id);
            });

        migrationBuilder.CreateIndex(
            name: "IX_MaterialReviews_MaterialId",
            schema: "ratings",
            table: "MaterialReviews",
            column: "MaterialId");

        migrationBuilder.CreateIndex(
            name: "IX_MaterialReviews_ReviewerId",
            schema: "ratings",
            table: "MaterialReviews",
            column: "ReviewerId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "MaterialReviews",
            schema: "ratings");

        migrationBuilder.CreateTable(
            name: "ArticleReviews",
            schema: "ratings",
            columns: table => new
            {
                Id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                ArticleId = table.Column<int>(type: "integer", nullable: false),
                CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                Rating = table.Column<int>(type: "integer", nullable: false),
                ReviewText = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                ReviewerId = table.Column<int>(type: "integer", nullable: false),
                UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ArticleReviews", x => x.Id);
            });

        migrationBuilder.CreateIndex(
            name: "IX_ArticleReviews_ArticleId",
            schema: "ratings",
            table: "ArticleReviews",
            column: "ArticleId");

        migrationBuilder.CreateIndex(
            name: "IX_ArticleReviews_ReviewerId",
            schema: "ratings",
            table: "ArticleReviews",
            column: "ReviewerId");
    }
}
