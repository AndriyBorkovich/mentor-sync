using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MentorSync.Users.Data.Migrations;

/// <inheritdoc />
public partial class AddRefreshTokenFieldsToAppUser : Migration
{
	/// <inheritdoc />
	protected override void Up(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.AddColumn<string>(
			name: "RefreshToken",
			schema: "users",
			table: "Users",
			type: "character varying(300)",
			maxLength: 300,
			nullable: true);

		migrationBuilder.AddColumn<DateTime>(
			name: "RefreshTokenExpiryTime",
			schema: "users",
			table: "Users",
			type: "timestamp with time zone",
			nullable: true);
	}

	/// <inheritdoc />
	protected override void Down(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.DropColumn(
			name: "RefreshToken",
			schema: "users",
			table: "Users");

		migrationBuilder.DropColumn(
			name: "RefreshTokenExpiryTime",
			schema: "users",
			table: "Users");
	}
}
