using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MentorSync.Recommendations.Data.Migrations;

/// <inheritdoc />
public partial class RemoveCommunicationsLanguages : Migration
{
	/// <inheritdoc />
	protected override void Up(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.DropColumn(
			name: "PreferredCommunicationLanguage",
			schema: "recommendations",
			table: "MenteePreferences");
	}

	/// <inheritdoc />
	protected override void Down(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.AddColumn<string>(
			name: "PreferredCommunicationLanguage",
			schema: "recommendations",
			table: "MenteePreferences",
			type: "text",
			nullable: true);
	}
}
