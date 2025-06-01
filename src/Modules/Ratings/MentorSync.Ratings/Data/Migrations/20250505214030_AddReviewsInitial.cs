using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MentorSync.Ratings.Data.Migrations;

/// <inheritdoc />
public partial class AddReviewsInitial : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.EnsureSchema(
            name: "ratings");

        migrationBuilder.CreateTable(
            name: "ArticleReviews",
            schema: "ratings",
            columns: table => new
            {
                Id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                ArticleId = table.Column<int>(type: "integer", nullable: false),
                ReviewerId = table.Column<int>(type: "integer", nullable: false),
                Rating = table.Column<int>(type: "integer", nullable: false),
                ReviewText = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ArticleReviews", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "MentorReviews",
            schema: "ratings",
            columns: table => new
            {
                Id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                MentorId = table.Column<int>(type: "integer", nullable: false),
                MenteeId = table.Column<int>(type: "integer", nullable: false),
                Rating = table.Column<int>(type: "integer", nullable: false),
                ReviewText = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_MentorReviews", x => x.Id);
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

        migrationBuilder.CreateIndex(
            name: "IX_MentorReviews_MenteeId",
            schema: "ratings",
            table: "MentorReviews",
            column: "MenteeId");

        migrationBuilder.CreateIndex(
            name: "IX_MentorReviews_MentorId",
            schema: "ratings",
            table: "MentorReviews",
            column: "MentorId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "ArticleReviews",
            schema: "ratings");

        migrationBuilder.DropTable(
            name: "MentorReviews",
            schema: "ratings");
    }
}
