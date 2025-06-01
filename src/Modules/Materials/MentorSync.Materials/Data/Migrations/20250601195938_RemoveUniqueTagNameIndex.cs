using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MentorSync.Materials.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemoveUniqueTagNameIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Tags_Name",
                schema: "materials",
                table: "Tags");

            migrationBuilder.CreateIndex(
                name: "IX_Tags_Name",
                schema: "materials",
                table: "Tags",
                column: "Name");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Tags_Name",
                schema: "materials",
                table: "Tags");

            migrationBuilder.CreateIndex(
                name: "IX_Tags_Name",
                schema: "materials",
                table: "Tags",
                column: "Name",
                unique: true);
        }
    }
}
