using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class v27 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Stories_AspNetUsers_UserId",
                table: "Stories");

            migrationBuilder.DropForeignKey(
                name: "FK_StoryUser_AspNetUsers_UserId",
                table: "StoryUser");

            migrationBuilder.DropForeignKey(
                name: "FK_StoryUser_Stories_StoryId",
                table: "StoryUser");

            migrationBuilder.DropIndex(
                name: "IX_Stories_UserId",
                table: "Stories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StoryUser",
                table: "StoryUser");

            migrationBuilder.RenameTable(
                name: "StoryUser",
                newName: "StoryUsers");

            migrationBuilder.RenameIndex(
                name: "IX_StoryUser_UserId",
                table: "StoryUsers",
                newName: "IX_StoryUsers_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_StoryUser_StoryId",
                table: "StoryUsers",
                newName: "IX_StoryUsers_StoryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StoryUsers",
                table: "StoryUsers",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_StoryUsers_AspNetUsers_UserId",
                table: "StoryUsers",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StoryUsers_Stories_StoryId",
                table: "StoryUsers",
                column: "StoryId",
                principalTable: "Stories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StoryUsers_AspNetUsers_UserId",
                table: "StoryUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_StoryUsers_Stories_StoryId",
                table: "StoryUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StoryUsers",
                table: "StoryUsers");

            migrationBuilder.RenameTable(
                name: "StoryUsers",
                newName: "StoryUser");

            migrationBuilder.RenameIndex(
                name: "IX_StoryUsers_UserId",
                table: "StoryUser",
                newName: "IX_StoryUser_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_StoryUsers_StoryId",
                table: "StoryUser",
                newName: "IX_StoryUser_StoryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StoryUser",
                table: "StoryUser",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Stories_UserId",
                table: "Stories",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Stories_AspNetUsers_UserId",
                table: "Stories",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StoryUser_AspNetUsers_UserId",
                table: "StoryUser",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StoryUser_Stories_StoryId",
                table: "StoryUser",
                column: "StoryId",
                principalTable: "Stories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
