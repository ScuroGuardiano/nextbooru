using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nextbooru.Core.Migrations
{
    /// <inheritdoc />
    public partial class ImagePublishedAt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "published_at",
                table: "images",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "published_at",
                table: "images");
        }
    }
}
