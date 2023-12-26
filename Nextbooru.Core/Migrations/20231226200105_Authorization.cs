using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nextbooru.Core.Migrations
{
    /// <inheritdoc />
    public partial class Authorization : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_album_users_created_by_id",
                table: "album");

            migrationBuilder.DropForeignKey(
                name: "fk_album_image_album_albums_id",
                table: "album_image");

            migrationBuilder.DropPrimaryKey(
                name: "pk_album",
                table: "album");

            migrationBuilder.RenameTable(
                name: "album",
                newName: "albums");

            migrationBuilder.RenameIndex(
                name: "ix_album_created_by_id",
                table: "albums",
                newName: "ix_albums_created_by_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_albums",
                table: "albums",
                column: "id");

            migrationBuilder.CreateTable(
                name: "roles",
                columns: table => new
                {
                    name = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_roles", x => x.name);
                });

            migrationBuilder.CreateTable(
                name: "user_permissions",
                columns: table => new
                {
                    permission_key = table.Column<string>(type: "text", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_permissions", x => new { x.user_id, x.permission_key });
                    table.ForeignKey(
                        name: "fk_user_permissions_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "role_permissions",
                columns: table => new
                {
                    permission_key = table.Column<string>(type: "text", nullable: false),
                    role_name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_role_permissions", x => new { x.role_name, x.permission_key });
                    table.ForeignKey(
                        name: "fk_role_permissions_roles_role_temp_id",
                        column: x => x.role_name,
                        principalTable: "roles",
                        principalColumn: "name",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_roles",
                columns: table => new
                {
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    role_name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_roles", x => new { x.role_name, x.user_id });
                    table.ForeignKey(
                        name: "fk_user_roles_roles_role_name",
                        column: x => x.role_name,
                        principalTable: "roles",
                        principalColumn: "name",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_user_roles_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_user_roles_user_id",
                table: "user_roles",
                column: "user_id");

            migrationBuilder.AddForeignKey(
                name: "fk_album_image_albums_albums_id",
                table: "album_image",
                column: "albums_id",
                principalTable: "albums",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_albums_users_created_by_id",
                table: "albums",
                column: "created_by_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_album_image_albums_albums_id",
                table: "album_image");

            migrationBuilder.DropForeignKey(
                name: "fk_albums_users_created_by_id",
                table: "albums");

            migrationBuilder.DropTable(
                name: "role_permissions");

            migrationBuilder.DropTable(
                name: "user_permissions");

            migrationBuilder.DropTable(
                name: "user_roles");

            migrationBuilder.DropTable(
                name: "roles");

            migrationBuilder.DropPrimaryKey(
                name: "pk_albums",
                table: "albums");

            migrationBuilder.RenameTable(
                name: "albums",
                newName: "album");

            migrationBuilder.RenameIndex(
                name: "ix_albums_created_by_id",
                table: "album",
                newName: "ix_album_created_by_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_album",
                table: "album",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_album_users_created_by_id",
                table: "album",
                column: "created_by_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_album_image_album_albums_id",
                table: "album_image",
                column: "albums_id",
                principalTable: "album",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
