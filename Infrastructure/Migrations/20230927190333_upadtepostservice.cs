using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class upadtepostservice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostCommentLikes_PostComments_PostCommentId",
                table: "PostCommentLikes");

            migrationBuilder.AlterColumn<string>(
                name: "Comment",
                table: "PostComments",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "PostCommentId",
                table: "PostCommentLikes",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<int>(
                name: "PostCommentId1",
                table: "PostCommentLikes",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_PostCommentLikes_PostCommentId1",
                table: "PostCommentLikes",
                column: "PostCommentId1");

            migrationBuilder.AddForeignKey(
                name: "FK_PostCommentLikes_PostComments_PostCommentId1",
                table: "PostCommentLikes",
                column: "PostCommentId1",
                principalTable: "PostComments",
                principalColumn: "PostCommentId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostCommentLikes_PostComments_PostCommentId1",
                table: "PostCommentLikes");

            migrationBuilder.DropIndex(
                name: "IX_PostCommentLikes_PostCommentId1",
                table: "PostCommentLikes");

            migrationBuilder.DropColumn(
                name: "PostCommentId1",
                table: "PostCommentLikes");

            migrationBuilder.AlterColumn<string>(
                name: "Comment",
                table: "PostComments",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<int>(
                name: "PostCommentId",
                table: "PostCommentLikes",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddForeignKey(
                name: "FK_PostCommentLikes_PostComments_PostCommentId",
                table: "PostCommentLikes",
                column: "PostCommentId",
                principalTable: "PostComments",
                principalColumn: "PostCommentId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
