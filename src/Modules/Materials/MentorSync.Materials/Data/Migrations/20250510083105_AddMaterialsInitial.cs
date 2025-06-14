using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MentorSync.Materials.Data.Migrations;

/// <inheritdoc />
public partial class AddMaterialsInitial : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.EnsureSchema(
            name: "materials");

        migrationBuilder.CreateTable(
            name: "LearningMaterials",
            schema: "materials",
            columns: table => new
            {
                Id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                Title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                Description = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                Type = table.Column<int>(type: "integer", nullable: false),
                ContentMarkdown = table.Column<string>(type: "text", nullable: true),
                MentorId = table.Column<int>(type: "integer", nullable: false),
                CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_LearningMaterials", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Tags",
            schema: "materials",
            columns: table => new
            {
                Id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                Name = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: false),
                Description = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Tags", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "MaterialAttachments",
            schema: "materials",
            columns: table => new
            {
                Id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                MaterialId = table.Column<int>(type: "integer", nullable: false),
                FileName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                BlobUri = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                ContentType = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                UploadedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_MaterialAttachments", x => x.Id);
                table.ForeignKey(
                    name: "FK_MaterialAttachments_LearningMaterials_MaterialId",
                    column: x => x.MaterialId,
                    principalSchema: "materials",
                    principalTable: "LearningMaterials",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "LearningMaterialTag",
            schema: "materials",
            columns: table => new
            {
                LearningMaterialsId = table.Column<int>(type: "integer", nullable: false),
                TagsId = table.Column<int>(type: "integer", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_LearningMaterialTag", x => new { x.LearningMaterialsId, x.TagsId });
                table.ForeignKey(
                    name: "FK_LearningMaterialTag_LearningMaterials_LearningMaterialsId",
                    column: x => x.LearningMaterialsId,
                    principalSchema: "materials",
                    principalTable: "LearningMaterials",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_LearningMaterialTag_Tags_TagsId",
                    column: x => x.TagsId,
                    principalSchema: "materials",
                    principalTable: "Tags",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_LearningMaterials_Title",
            schema: "materials",
            table: "LearningMaterials",
            column: "Title");

        migrationBuilder.CreateIndex(
            name: "IX_LearningMaterialTag_TagsId",
            schema: "materials",
            table: "LearningMaterialTag",
            column: "TagsId");

        migrationBuilder.CreateIndex(
            name: "IX_MaterialAttachments_MaterialId",
            schema: "materials",
            table: "MaterialAttachments",
            column: "MaterialId");

        migrationBuilder.CreateIndex(
            name: "IX_Tags_Name",
            schema: "materials",
            table: "Tags",
            column: "Name",
            unique: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "LearningMaterialTag",
            schema: "materials");

        migrationBuilder.DropTable(
            name: "MaterialAttachments",
            schema: "materials");

        migrationBuilder.DropTable(
            name: "Tags",
            schema: "materials");

        migrationBuilder.DropTable(
            name: "LearningMaterials",
            schema: "materials");
    }
}
