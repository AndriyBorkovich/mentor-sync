using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MentorSync.Users.Data.Migrations
{
    /// <inheritdoc />
    public partial class MoveBioFromAppUserToProfilesTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Bio",
                schema: "users",
                table: "Users");

            migrationBuilder.AddColumn<string>(
                name: "Bio",
                schema: "users",
                table: "MentorProfiles",
                type: "character varying(2000)",
                maxLength: 2000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Bio",
                schema: "users",
                table: "MenteeProfiles",
                type: "character varying(2000)",
                maxLength: 2000,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Bio",
                schema: "users",
                table: "MentorProfiles");

            migrationBuilder.DropColumn(
                name: "Bio",
                schema: "users",
                table: "MenteeProfiles");

            migrationBuilder.AddColumn<string>(
                name: "Bio",
                schema: "users",
                table: "Users",
                type: "character varying(2000)",
                maxLength: 2000,
                nullable: true);
        }
    }
}
