using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MentorSync.Users.Data.Migrations;

/// <inheritdoc />
public partial class RemoveCommunicationsLanguages : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "CommunicationLanguages",
            schema: "users",
            table: "Users");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<List<string>>(
            name: "CommunicationLanguages",
            schema: "users",
            table: "Users",
            type: "text[]",
            nullable: true);
    }
}
