using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MentorSync.Users.Data.Migrations
{
	/// <inheritdoc />
	public partial class AddCompanyAndPositionColumnsToProfiles : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.AddColumn<string>(
				name: "Company",
				schema: "users",
				table: "MentorProfiles",
				type: "character varying(100)",
				maxLength: 100,
				nullable: true);

			migrationBuilder.AddColumn<string>(
				name: "Position",
				schema: "users",
				table: "MentorProfiles",
				type: "character varying(100)",
				maxLength: 100,
				nullable: true);

			migrationBuilder.Sql(@"
                ALTER TABLE users.""MenteeProfiles""
                ALTER COLUMN ""LearningGoals"" TYPE text[]
                USING ""LearningGoals""::text[];");

			migrationBuilder.AddColumn<string>(
				name: "Company",
				schema: "users",
				table: "MenteeProfiles",
				type: "character varying(100)",
				maxLength: 100,
				nullable: true);

			migrationBuilder.AddColumn<string>(
				name: "Position",
				schema: "users",
				table: "MenteeProfiles",
				type: "character varying(100)",
				maxLength: 100,
				nullable: true);
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropColumn(
				name: "Company",
				schema: "users",
				table: "MentorProfiles");

			migrationBuilder.DropColumn(
				name: "Position",
				schema: "users",
				table: "MentorProfiles");

			migrationBuilder.DropColumn(
				name: "Company",
				schema: "users",
				table: "MenteeProfiles");

			migrationBuilder.DropColumn(
				name: "Position",
				schema: "users",
				table: "MenteeProfiles");

			migrationBuilder.Sql(@"
                ALTER TABLE users.""MenteeProfiles""
                ALTER COLUMN ""LearningGoals"" TYPE character varying(200)
                USING array_to_string(""LearningGoals"", ','); ");
		}
	}
}
