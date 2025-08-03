using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MentorSync.Recommendations.Data.Migrations;

/// <inheritdoc />
public sealed partial class AddedMaterialsRelatedTables : Migration
{
	/// <inheritdoc />
	protected override void Up(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.DropTable(
			name: "RecommendationResults",
			schema: "recommendations");

		migrationBuilder.CreateTable(
			name: "MaterialLikes",
			schema: "recommendations",
			columns: table => new
			{
				Id = table.Column<int>(type: "integer", nullable: false)
					.Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
				MenteeId = table.Column<int>(type: "integer", nullable: false),
				MaterialId = table.Column<int>(type: "integer", nullable: false),
				LikedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
			},
			constraints: table =>
			{
				table.PrimaryKey("PK_MaterialLikes", x => x.Id);
			});

		migrationBuilder.CreateTable(
			name: "MaterialRecommendationResults",
			schema: "recommendations",
			columns: table => new
			{
				Id = table.Column<int>(type: "integer", nullable: false)
					.Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
				MaterialId = table.Column<int>(type: "integer", nullable: false),
				MenteeId = table.Column<int>(type: "integer", nullable: false),
				CollaborativeScore = table.Column<float>(type: "real", nullable: false),
				ContentBasedScore = table.Column<float>(type: "real", nullable: false),
				FinalScore = table.Column<float>(type: "real", nullable: false),
				GeneratedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
			},
			constraints: table =>
			{
				table.PrimaryKey("PK_MaterialRecommendationResults", x => x.Id);
			});

		migrationBuilder.CreateTable(
			name: "MaterialViewEvents",
			schema: "recommendations",
			columns: table => new
			{
				Id = table.Column<int>(type: "integer", nullable: false)
					.Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
				MaterialId = table.Column<int>(type: "integer", nullable: false),
				MenteeId = table.Column<int>(type: "integer", nullable: false),
				ViewedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
			},
			constraints: table =>
			{
				table.PrimaryKey("PK_MaterialViewEvents", x => x.Id);
			});

		migrationBuilder.CreateTable(
			name: "MenteeMaterialInteractions",
			schema: "recommendations",
			columns: table => new
			{
				Id = table.Column<int>(type: "integer", nullable: false)
					.Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
				MaterialId = table.Column<int>(type: "integer", nullable: false),
				MenteeId = table.Column<int>(type: "integer", nullable: false),
				Score = table.Column<float>(type: "real", nullable: false),
				UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
			},
			constraints: table =>
			{
				table.PrimaryKey("PK_MenteeMaterialInteractions", x => x.Id);
			});

		migrationBuilder.CreateTable(
			name: "MentorRecommendationResults",
			schema: "recommendations",
			columns: table => new
			{
				Id = table.Column<int>(type: "integer", nullable: false)
					.Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
				MentorId = table.Column<int>(type: "integer", nullable: false),
				MenteeId = table.Column<int>(type: "integer", nullable: false),
				CollaborativeScore = table.Column<float>(type: "real", nullable: false),
				ContentBasedScore = table.Column<float>(type: "real", nullable: false),
				FinalScore = table.Column<float>(type: "real", nullable: false),
				GeneratedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
			},
			constraints: table =>
			{
				table.PrimaryKey("PK_MentorRecommendationResults", x => x.Id);
			});
	}

	/// <inheritdoc />
	protected override void Down(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.DropTable(
			name: "MaterialLikes",
			schema: "recommendations");

		migrationBuilder.DropTable(
			name: "MaterialRecommendationResults",
			schema: "recommendations");

		migrationBuilder.DropTable(
			name: "MaterialViewEvents",
			schema: "recommendations");

		migrationBuilder.DropTable(
			name: "MenteeMaterialInteractions",
			schema: "recommendations");

		migrationBuilder.DropTable(
			name: "MentorRecommendationResults",
			schema: "recommendations");

		migrationBuilder.CreateTable(
			name: "RecommendationResults",
			schema: "recommendations",
			columns: table => new
			{
				Id = table.Column<int>(type: "integer", nullable: false)
					.Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
				CollaborativeScore = table.Column<float>(type: "real", nullable: false),
				ContentBasedScore = table.Column<float>(type: "real", nullable: false),
				FinalScore = table.Column<float>(type: "real", nullable: false),
				GeneratedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
				MenteeId = table.Column<int>(type: "integer", nullable: false),
				MentorId = table.Column<int>(type: "integer", nullable: false)
			},
			constraints: table =>
			{
				table.PrimaryKey("PK_RecommendationResults", x => x.Id);
			});
	}
}
