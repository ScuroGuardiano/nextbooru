using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Nextbooru.Core.Migrations
{
    /// <inheritdoc />
    public partial class ImageVariants : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "image_variants",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    image_id = table.Column<long>(type: "bigint", nullable: false),
                    store_file_id = table.Column<string>(type: "text", nullable: false),
                    width = table.Column<int>(type: "integer", nullable: false),
                    height = table.Column<int>(type: "integer", nullable: false),
                    extension = table.Column<string>(type: "text", nullable: true),
                    content_type = table.Column<string>(type: "text", nullable: true),
                    variant_mode = table.Column<int>(type: "integer", nullable: false),
                    size_in_bytes = table.Column<long>(type: "bigint", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_image_variants", x => x.id);
                    table.ForeignKey(
                        name: "fk_image_variants_images_image_id",
                        column: x => x.image_id,
                        principalTable: "images",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_image_variants_image_id",
                table: "image_variants",
                column: "image_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "image_variants");
        }
    }
}
