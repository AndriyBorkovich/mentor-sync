using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MentorSync.Users.Data.Migrations;

/// <inheritdoc />
public partial class RestructureProfilesTables : Migration
{
	/// <inheritdoc />
	protected override void Up(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.RenameColumn(
			name: "PreferredMentorIndustry",
			schema: "users",
			table: "MenteeProfiles",
			newName: "Industries");

		migrationBuilder.RenameColumn(
			name: "PreferredLanguages",
			schema: "users",
			table: "MenteeProfiles",
			newName: "Skills");

		migrationBuilder.RenameColumn(
			name: "DesiredSkills",
			schema: "users",
			table: "MenteeProfiles",
			newName: "ProgrammingLanguages");
	}

	/// <inheritdoc />
	protected override void Down(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.RenameColumn(
			name: "Skills",
			schema: "users",
			table: "MenteeProfiles",
			newName: "PreferredLanguages");

		migrationBuilder.RenameColumn(
			name: "ProgrammingLanguages",
			schema: "users",
			table: "MenteeProfiles",
			newName: "DesiredSkills");

		migrationBuilder.RenameColumn(
			name: "Industries",
			schema: "users",
			table: "MenteeProfiles",
			newName: "PreferredMentorIndustry");
	}
}
