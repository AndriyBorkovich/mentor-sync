using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MentorSync.Users.Data.Migrations;

/// <inheritdoc />
public partial class CorrectRelationBetweenProfilesAndUser : Migration
{
	/// <inheritdoc />
	protected override void Up(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.DropForeignKey(
			name: "FK_MenteeProfiles_Users_UserId",
			schema: "users",
			table: "MenteeProfiles");

		migrationBuilder.DropForeignKey(
			name: "FK_MentorProfiles_Users_UserId",
			schema: "users",
			table: "MentorProfiles");

		migrationBuilder.DropIndex(
			name: "IX_MentorProfiles_UserId",
			schema: "users",
			table: "MentorProfiles");

		migrationBuilder.DropIndex(
			name: "IX_MenteeProfiles_UserId",
			schema: "users",
			table: "MenteeProfiles");

		migrationBuilder.DropColumn(
			name: "UserId",
			schema: "users",
			table: "MentorProfiles");

		migrationBuilder.DropColumn(
			name: "UserId",
			schema: "users",
			table: "MenteeProfiles");

		migrationBuilder.CreateIndex(
			name: "IX_MentorProfiles_MentorId",
			schema: "users",
			table: "MentorProfiles",
			column: "MentorId",
			unique: true);

		migrationBuilder.CreateIndex(
			name: "IX_MenteeProfiles_MenteeId",
			schema: "users",
			table: "MenteeProfiles",
			column: "MenteeId",
			unique: true);

		migrationBuilder.AddForeignKey(
			name: "FK_MenteeProfiles_Users_MenteeId",
			schema: "users",
			table: "MenteeProfiles",
			column: "MenteeId",
			principalSchema: "users",
			principalTable: "Users",
			principalColumn: "Id",
			onDelete: ReferentialAction.Cascade);

		migrationBuilder.AddForeignKey(
			name: "FK_MentorProfiles_Users_MentorId",
			schema: "users",
			table: "MentorProfiles",
			column: "MentorId",
			principalSchema: "users",
			principalTable: "Users",
			principalColumn: "Id",
			onDelete: ReferentialAction.Cascade);
	}

	/// <inheritdoc />
	protected override void Down(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.DropForeignKey(
			name: "FK_MenteeProfiles_Users_MenteeId",
			schema: "users",
			table: "MenteeProfiles");

		migrationBuilder.DropForeignKey(
			name: "FK_MentorProfiles_Users_MentorId",
			schema: "users",
			table: "MentorProfiles");

		migrationBuilder.DropIndex(
			name: "IX_MentorProfiles_MentorId",
			schema: "users",
			table: "MentorProfiles");

		migrationBuilder.DropIndex(
			name: "IX_MenteeProfiles_MenteeId",
			schema: "users",
			table: "MenteeProfiles");

		migrationBuilder.AddColumn<int>(
			name: "UserId",
			schema: "users",
			table: "MentorProfiles",
			type: "integer",
			nullable: true);

		migrationBuilder.AddColumn<int>(
			name: "UserId",
			schema: "users",
			table: "MenteeProfiles",
			type: "integer",
			nullable: true);

		migrationBuilder.CreateIndex(
			name: "IX_MentorProfiles_UserId",
			schema: "users",
			table: "MentorProfiles",
			column: "UserId");

		migrationBuilder.CreateIndex(
			name: "IX_MenteeProfiles_UserId",
			schema: "users",
			table: "MenteeProfiles",
			column: "UserId");

		migrationBuilder.AddForeignKey(
			name: "FK_MenteeProfiles_Users_UserId",
			schema: "users",
			table: "MenteeProfiles",
			column: "UserId",
			principalSchema: "users",
			principalTable: "Users",
			principalColumn: "Id");

		migrationBuilder.AddForeignKey(
			name: "FK_MentorProfiles_Users_UserId",
			schema: "users",
			table: "MentorProfiles",
			column: "UserId",
			principalSchema: "users",
			principalTable: "Users",
			principalColumn: "Id");
	}
}
