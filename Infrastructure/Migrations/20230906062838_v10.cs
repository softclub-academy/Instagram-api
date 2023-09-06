using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class v10 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StoryStats_Stories_StoryId",
                table: "StoryStats");

            migrationBuilder.DropColumn(
                name: "LikeCount",
                table: "StoryStats");

            migrationBuilder.AlterColumn<int>(
                name: "StoryId",
                table: "StoryStats",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_StoryStats_Stories_StoryId",
                table: "StoryStats",
                column: "StoryId",
                principalTable: "Stories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StoryStats_Stories_StoryId",
                table: "StoryStats");

            migrationBuilder.AlterColumn<int>(
                name: "StoryId",
                table: "StoryStats",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<int>(
                name: "LikeCount",
                table: "StoryStats",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_StoryStats_Stories_StoryId",
                table: "StoryStats",
                column: "StoryId",
                principalTable: "Stories",
                principalColumn: "Id");
        }
    }
}
