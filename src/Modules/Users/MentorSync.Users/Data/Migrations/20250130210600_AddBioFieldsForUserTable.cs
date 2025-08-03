using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MentorSync.Users.Data.Migrations;

/// <inheritdoc />
public partial class AddBioFieldsForUserTable : Migration
{
	/// <inheritdoc />
	protected override void Up(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.AlterColumn<string>(
			name: "ProfileImageUrl",
			schema: "users",
			table: "Users",
			type: "character varying(500)",
			maxLength: 500,
			nullable: true,
			oldClrType: typeof(string),
			oldType: "character varying(500)",
			oldMaxLength: 500);

		migrationBuilder.AddColumn<string>(
			name: "Bio",
			schema: "users",
			table: "Users",
			type: "character varying(2000)",
			maxLength: 2000,
			nullable: true);

		migrationBuilder.AddColumn<List<string>>(
			name: "CommunicationLanguages",
			schema: "users",
			table: "Users",
			type: "text[]",
			nullable: true);

		migrationBuilder.AddColumn<string>(
			name: "Country",
			schema: "users",
			table: "Users",
			type: "character varying(30)",
			maxLength: 30,
			nullable: true);
	}

	/// <inheritdoc />
	protected override void Down(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.DropColumn(
			name: "Bio",
			schema: "users",
			table: "Users");

		migrationBuilder.DropColumn(
			name: "CommunicationLanguages",
			schema: "users",
			table: "Users");

		migrationBuilder.DropColumn(
			name: "Country",
			schema: "users",
			table: "Users");

		migrationBuilder.AlterColumn<string>(
			name: "ProfileImageUrl",
			schema: "users",
			table: "Users",
			type: "character varying(500)",
			maxLength: 500,
			nullable: false,
			defaultValue: "",
			oldClrType: typeof(string),
			oldType: "character varying(500)",
			oldMaxLength: 500,
			oldNullable: true);
	}
}
