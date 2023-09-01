using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class removepostviewid1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostViews_Posts_PostId1",
                table: "PostViews");

            migrationBuilder.DropIndex(
                name: "IX_PostViews_PostId1",
                table: "PostViews");

            migrationBuilder.DropColumn(
                name: "PostId1",
                table: "PostViews");

            migrationBuilder.AlterColumn<int>(
                name: "PostId",
                table: "PostViews",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddForeignKey(
                name: "FK_PostViews_Posts_PostId",
                table: "PostViews",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "PostId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostViews_Posts_PostId",
                table: "PostViews");

            migrationBuilder.AlterColumn<int>(
                name: "PostId",
                table: "PostViews",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<int>(
                name: "PostId1",
                table: "PostViews",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_PostViews_PostId1",
                table: "PostViews",
                column: "PostId1");

            migrationBuilder.AddForeignKey(
                name: "FK_PostViews_Posts_PostId1",
                table: "PostViews",
                column: "PostId1",
                principalTable: "Posts",
                principalColumn: "PostId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
