using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nextbooru.Core.Migrations
{
    /// <inheritdoc />
    public partial class Image : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Images");

            migrationBuilder.RenameColumn(
                name: "Url",
                table: "Images",
                newName: "StoreFileId");

            migrationBuilder.AddColumn<string>(
                name: "ContentType",
                table: "Images",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Extension",
                table: "Images",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Height",
                table: "Images",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsPublic",
                table: "Images",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<long>(
                name: "SizeInBytes",
                table: "Images",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "Source",
                table: "Images",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Images",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UploadedById",
                table: "Images",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<int>(
                name: "Width",
                table: "Images",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Tag",
                columns: table => new
                {
                    Name = table.Column<string>(type: "text", nullable: false),
                    TagType = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tag", x => x.Name);
                });

            migrationBuilder.CreateTable(
                name: "ImageTag",
                columns: table => new
                {
                    ImagesId = table.Column<long>(type: "bigint", nullable: false),
                    TagsName = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImageTag", x => new { x.ImagesId, x.TagsName });
                    table.ForeignKey(
                        name: "FK_ImageTag_Images_ImagesId",
                        column: x => x.ImagesId,
                        principalTable: "Images",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ImageTag_Tag_TagsName",
                        column: x => x.TagsName,
                        principalTable: "Tag",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Images_UploadedById",
                table: "Images",
                column: "UploadedById");

            migrationBuilder.CreateIndex(
                name: "IX_ImageTag_TagsName",
                table: "ImageTag",
                column: "TagsName");

            migrationBuilder.AddForeignKey(
                name: "FK_Images_Users_UploadedById",
                table: "Images",
                column: "UploadedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Images_Users_UploadedById",
                table: "Images");

            migrationBuilder.DropTable(
                name: "ImageTag");

            migrationBuilder.DropTable(
                name: "Tag");

            migrationBuilder.DropIndex(
                name: "IX_Images_UploadedById",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "ContentType",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "Extension",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "Height",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "IsPublic",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "SizeInBytes",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "Source",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "UploadedById",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "Width",
                table: "Images");

            migrationBuilder.RenameColumn(
                name: "StoreFileId",
                table: "Images",
                newName: "Url");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Images",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
