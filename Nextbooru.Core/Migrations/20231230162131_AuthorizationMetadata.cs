using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nextbooru.Core.Migrations
{
    /// <inheritdoc />
    public partial class AuthorizationMetadata : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "builtin",
                table: "roles",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "authorization_metadata",
                columns: table => new
                {
                    builtin_roles_initialized = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "authorization_metadata");

            migrationBuilder.DropColumn(
                name: "builtin",
                table: "roles");
        }
    }
}
