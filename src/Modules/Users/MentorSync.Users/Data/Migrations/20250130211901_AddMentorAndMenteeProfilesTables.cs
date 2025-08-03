using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MentorSync.Users.Data.Migrations
{
	/// <inheritdoc />
	public partial class AddMentorAndMenteeProfilesTables : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.CreateTable(
				name: "MenteeProfiles",
				schema: "users",
				columns: table => new
				{
					Id = table.Column<int>(type: "integer", nullable: false)
						.Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
					DesiredSkills = table.Column<List<string>>(type: "text[]", nullable: true),
					LearningGoals = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
					PreferredMentorIndustry = table.Column<int>(type: "integer", nullable: false),
					PreferredLanguages = table.Column<List<string>>(type: "text[]", nullable: true),
					MenteeId = table.Column<int>(type: "integer", nullable: false),
					UserId = table.Column<int>(type: "integer", nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_MenteeProfiles", x => x.Id);
					table.ForeignKey(
						name: "FK_MenteeProfiles_Users_UserId",
						column: x => x.UserId,
						principalSchema: "users",
						principalTable: "Users",
						principalColumn: "Id");
				});

			migrationBuilder.CreateTable(
				name: "MentorProfiles",
				schema: "users",
				columns: table => new
				{
					Id = table.Column<int>(type: "integer", nullable: false)
						.Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
					Industries = table.Column<int>(type: "integer", nullable: false),
					Skills = table.Column<List<string>>(type: "text[]", nullable: true),
					ProgrammingLanguages = table.Column<List<string>>(type: "text[]", nullable: true),
					ExperienceYears = table.Column<int>(type: "integer", nullable: false),
					Availability = table.Column<int>(type: "integer", nullable: false),
					MentorId = table.Column<int>(type: "integer", nullable: false),
					UserId = table.Column<int>(type: "integer", nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_MentorProfiles", x => x.Id);
					table.ForeignKey(
						name: "FK_MentorProfiles_Users_UserId",
						column: x => x.UserId,
						principalSchema: "users",
						principalTable: "Users",
						principalColumn: "Id");
				});

			migrationBuilder.CreateIndex(
				name: "IX_MenteeProfiles_UserId",
				schema: "users",
				table: "MenteeProfiles",
				column: "UserId");

			migrationBuilder.CreateIndex(
				name: "IX_MentorProfiles_UserId",
				schema: "users",
				table: "MentorProfiles",
				column: "UserId");
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
				name: "MenteeProfiles",
				schema: "users");

			migrationBuilder.DropTable(
				name: "MentorProfiles",
				schema: "users");
		}
	}
}
