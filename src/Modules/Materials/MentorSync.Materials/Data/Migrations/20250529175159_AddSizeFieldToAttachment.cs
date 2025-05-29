using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MentorSync.Materials.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddSizeFieldToAttachment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "Size",
                schema: "materials",
                table: "MaterialAttachments",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Size",
                schema: "materials",
                table: "MaterialAttachments");
        }
    }
}
