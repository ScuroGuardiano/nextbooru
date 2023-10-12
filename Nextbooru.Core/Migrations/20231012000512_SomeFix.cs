using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nextbooru.Core.Migrations
{
    /// <inheritdoc />
    public partial class SomeFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ImageTag_Tag_TagsName",
                table: "ImageTag");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tag",
                table: "Tag");

            migrationBuilder.RenameTable(
                name: "Tag",
                newName: "Tags");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tags",
                table: "Tags",
                column: "Name");

            migrationBuilder.AddForeignKey(
                name: "FK_ImageTag_Tags_TagsName",
                table: "ImageTag",
                column: "TagsName",
                principalTable: "Tags",
                principalColumn: "Name",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ImageTag_Tags_TagsName",
                table: "ImageTag");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tags",
                table: "Tags");

            migrationBuilder.RenameTable(
                name: "Tags",
                newName: "Tag");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tag",
                table: "Tag",
                column: "Name");

            migrationBuilder.AddForeignKey(
                name: "FK_ImageTag_Tag_TagsName",
                table: "ImageTag",
                column: "TagsName",
                principalTable: "Tag",
                principalColumn: "Name",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
