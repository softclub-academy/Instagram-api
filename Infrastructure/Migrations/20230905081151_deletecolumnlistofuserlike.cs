using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class deletecolumnlistofuserlike : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostUserLikes_ListOfUserCommentLikes_ListOfUserCommentLikeId",
                table: "PostUserLikes");

            migrationBuilder.DropIndex(
                name: "IX_PostUserLikes_ListOfUserCommentLikeId",
                table: "PostUserLikes");

            migrationBuilder.DropColumn(
                name: "ListOfUserCommentLikeId",
                table: "PostUserLikes");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ListOfUserCommentLikeId",
                table: "PostUserLikes",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PostUserLikes_ListOfUserCommentLikeId",
                table: "PostUserLikes",
                column: "ListOfUserCommentLikeId");

            migrationBuilder.AddForeignKey(
                name: "FK_PostUserLikes_ListOfUserCommentLikes_ListOfUserCommentLikeId",
                table: "PostUserLikes",
                column: "ListOfUserCommentLikeId",
                principalTable: "ListOfUserCommentLikes",
                principalColumn: "Id");
        }
    }
}
