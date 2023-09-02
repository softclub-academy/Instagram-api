using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class renametables1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.RenameColumn(
                name: "PostStatId",
                table: "StatUserIds",
                newName: "PostLikeId");

            migrationBuilder.CreateIndex(
                name: "IX_StatUserIds_PostLikeId",
                table: "StatUserIds",
                column: "PostLikeId");

            migrationBuilder.AddForeignKey(
                name: "FK_StatUserIds_PostStats_PostLikeId",
                table: "StatUserIds",
                column: "PostLikeId",
                principalTable: "PostStats",
                principalColumn: "PostId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StatUserIds_PostStats_PostLikeId",
                table: "StatUserIds");

            migrationBuilder.DropIndex(
                name: "IX_StatUserIds_PostLikeId",
                table: "StatUserIds");

            migrationBuilder.RenameColumn(
                name: "PostLikeId",
                table: "StatUserIds",
                newName: "PostStatId");

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
    }
}
