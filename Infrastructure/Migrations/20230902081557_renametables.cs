using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class renametables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StatUserIds_PostStats_PostStatId",
                table: "StatUserIds");

            migrationBuilder.DropIndex(
                name: "IX_StatUserIds_PostStatId",
                table: "StatUserIds");

            migrationBuilder.AddColumn<int>(
                name: "PostLikePostId",
                table: "StatUserIds",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_StatUserIds_PostLikePostId",
                table: "StatUserIds",
                column: "PostLikePostId");

            migrationBuilder.AddForeignKey(
                name: "FK_StatUserIds_PostStats_PostLikePostId",
                table: "StatUserIds",
                column: "PostLikePostId",
                principalTable: "PostStats",
                principalColumn: "PostId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StatUserIds_PostStats_PostLikePostId",
                table: "StatUserIds");

            migrationBuilder.DropIndex(
                name: "IX_StatUserIds_PostLikePostId",
                table: "StatUserIds");

            migrationBuilder.DropColumn(
                name: "PostLikePostId",
                table: "StatUserIds");

            migrationBuilder.CreateIndex(
                name: "IX_StatUserIds_PostStatId",
                table: "StatUserIds",
                column: "PostStatId");

            migrationBuilder.AddForeignKey(
                name: "FK_StatUserIds_PostStats_PostStatId",
                table: "StatUserIds",
                column: "PostStatId",
                principalTable: "PostStats",
                principalColumn: "PostId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
