using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class PostCommentLikes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PostCommentLikes",
                columns: table => new
                {
                    PostCommentId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    LikeCount = table.Column<int>(type: "integer", nullable: false),
                    PostCommentId1 = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostCommentLikes", x => x.PostCommentId);
                    table.ForeignKey(
                        name: "FK_PostCommentLikes_PostComments_PostCommentId1",
                        column: x => x.PostCommentId1,
                        principalTable: "PostComments",
                        principalColumn: "PostCommentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ListOfUserCommentLikes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PostCommentLikeId = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ListOfUserCommentLikes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ListOfUserCommentLikes_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ListOfUserCommentLikes_PostCommentLikes_PostCommentLikeId",
                        column: x => x.PostCommentLikeId,
                        principalTable: "PostCommentLikes",
                        principalColumn: "PostCommentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ListOfUserCommentLikes_PostCommentLikeId",
                table: "ListOfUserCommentLikes",
                column: "PostCommentLikeId");

            migrationBuilder.CreateIndex(
                name: "IX_ListOfUserCommentLikes_UserId",
                table: "ListOfUserCommentLikes",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_PostCommentLikes_PostCommentId1",
                table: "PostCommentLikes",
                column: "PostCommentId1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ListOfUserCommentLikes");

            migrationBuilder.DropTable(
                name: "PostCommentLikes");
        }
    }
}
