using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class upadtepostservice7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostFavorites_Posts_PostId1",
                table: "PostFavorites");

            migrationBuilder.DropIndex(
                name: "IX_PostFavorites_PostId1",
                table: "PostFavorites");

            migrationBuilder.DropColumn(
                name: "PostId1",
                table: "PostFavorites");

            migrationBuilder.AlterColumn<int>(
                name: "PostId",
                table: "PostFavorites",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddForeignKey(
                name: "FK_PostFavorites_Posts_PostId",
                table: "PostFavorites",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "PostId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostFavorites_Posts_PostId",
                table: "PostFavorites");

            migrationBuilder.AlterColumn<int>(
                name: "PostId",
                table: "PostFavorites",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<int>(
                name: "PostId1",
                table: "PostFavorites",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_PostFavorites_PostId1",
                table: "PostFavorites",
                column: "PostId1");

            migrationBuilder.AddForeignKey(
                name: "FK_PostFavorites_Posts_PostId1",
                table: "PostFavorites",
                column: "PostId1",
                principalTable: "Posts",
                principalColumn: "PostId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
