using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class upadtepostservice3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostFavorites_AspNetUsers_UserId",
                table: "PostFavorites");

            migrationBuilder.DropForeignKey(
                name: "FK_PostFavorites_Posts_PostId",
                table: "PostFavorites");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PostFavorites",
                table: "PostFavorites");

            migrationBuilder.DropIndex(
                name: "IX_PostFavorites_PostId",
                table: "PostFavorites");

            migrationBuilder.DropColumn(
                name: "DateFavorited",
                table: "PostFavorites");

            migrationBuilder.RenameColumn(
                name: "PostFavoriteId",
                table: "PostFavorites",
                newName: "PostId1");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "PostFavorites",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<int>(
                name: "PostId",
                table: "PostFavorites",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<int>(
                name: "PostId1",
                table: "PostFavorites",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<int>(
                name: "FavoriteCount",
                table: "PostFavorites",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_PostFavorites",
                table: "PostFavorites",
                column: "PostId");

            migrationBuilder.CreateTable(
                name: "PostFavoriteUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    PostFavoriteId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostFavoriteUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PostFavoriteUsers_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PostFavoriteUsers_PostFavorites_PostFavoriteId",
                        column: x => x.PostFavoriteId,
                        principalTable: "PostFavorites",
                        principalColumn: "PostId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PostFavorites_PostId1",
                table: "PostFavorites",
                column: "PostId1");

            migrationBuilder.CreateIndex(
                name: "IX_PostFavoriteUsers_PostFavoriteId",
                table: "PostFavoriteUsers",
                column: "PostFavoriteId");

            migrationBuilder.CreateIndex(
                name: "IX_PostFavoriteUsers_UserId",
                table: "PostFavoriteUsers",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_PostFavorites_AspNetUsers_UserId",
                table: "PostFavorites",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PostFavorites_Posts_PostId1",
                table: "PostFavorites",
                column: "PostId1",
                principalTable: "Posts",
                principalColumn: "PostId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostFavorites_AspNetUsers_UserId",
                table: "PostFavorites");

            migrationBuilder.DropForeignKey(
                name: "FK_PostFavorites_Posts_PostId1",
                table: "PostFavorites");

            migrationBuilder.DropTable(
                name: "PostFavoriteUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PostFavorites",
                table: "PostFavorites");

            migrationBuilder.DropIndex(
                name: "IX_PostFavorites_PostId1",
                table: "PostFavorites");

            migrationBuilder.DropColumn(
                name: "FavoriteCount",
                table: "PostFavorites");

            migrationBuilder.RenameColumn(
                name: "PostId1",
                table: "PostFavorites",
                newName: "PostFavoriteId");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "PostFavorites",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "PostId",
                table: "PostFavorites",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<int>(
                name: "PostFavoriteId",
                table: "PostFavorites",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateFavorited",
                table: "PostFavorites",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddPrimaryKey(
                name: "PK_PostFavorites",
                table: "PostFavorites",
                column: "PostFavoriteId");

            migrationBuilder.CreateIndex(
                name: "IX_PostFavorites_PostId",
                table: "PostFavorites",
                column: "PostId");

            migrationBuilder.AddForeignKey(
                name: "FK_PostFavorites_AspNetUsers_UserId",
                table: "PostFavorites",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PostFavorites_Posts_PostId",
                table: "PostFavorites",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "PostId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
