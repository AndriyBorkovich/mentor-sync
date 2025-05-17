using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MentorSync.Recommendations.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddRecommendationTablesInitial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "recommendations");

            migrationBuilder.CreateTable(
                name: "MenteeMentorInteractions",
                schema: "recommendations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MenteeId = table.Column<int>(type: "integer", nullable: false),
                    MentorId = table.Column<int>(type: "integer", nullable: false),
                    Score = table.Column<float>(type: "real", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenteeMentorInteractions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MenteePreferences",
                schema: "recommendations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MenteeId = table.Column<int>(type: "integer", nullable: false),
                    DesiredIndustries = table.Column<int>(type: "integer", nullable: false),
                    DesiredProgrammingLanguages = table.Column<List<string>>(type: "text[]", nullable: true),
                    MinMentorExperienceYears = table.Column<int>(type: "integer", nullable: true),
                    PreferredCommunicationLanguage = table.Column<string>(type: "text", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenteePreferences", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MentorBookmarks",
                schema: "recommendations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MenteeId = table.Column<int>(type: "integer", nullable: false),
                    MentorId = table.Column<int>(type: "integer", nullable: false),
                    BookmarkedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MentorBookmarks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MentorViewEvents",
                schema: "recommendations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MenteeId = table.Column<int>(type: "integer", nullable: false),
                    MentorId = table.Column<int>(type: "integer", nullable: false),
                    ViewedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MentorViewEvents", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RecommendationResults",
                schema: "recommendations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MenteeId = table.Column<int>(type: "integer", nullable: false),
                    MentorId = table.Column<int>(type: "integer", nullable: false),
                    CollaborativeScore = table.Column<float>(type: "real", nullable: false),
                    ContentBasedScore = table.Column<float>(type: "real", nullable: false),
                    FinalScore = table.Column<float>(type: "real", nullable: false),
                    GeneratedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecommendationResults", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MenteeMentorInteractions",
                schema: "recommendations");

            migrationBuilder.DropTable(
                name: "MenteePreferences",
                schema: "recommendations");

            migrationBuilder.DropTable(
                name: "MentorBookmarks",
                schema: "recommendations");

            migrationBuilder.DropTable(
                name: "MentorViewEvents",
                schema: "recommendations");

            migrationBuilder.DropTable(
                name: "RecommendationResults",
                schema: "recommendations");
        }
    }
}
