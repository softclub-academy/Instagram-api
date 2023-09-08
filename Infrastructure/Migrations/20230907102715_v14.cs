using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class v14 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ViewerDto");

            migrationBuilder.AddColumn<int>(
                name: "StoryId",
                table: "Viewers",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Viewers_StoryId",
                table: "Viewers",
                column: "StoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Viewers_Stories_StoryId",
                table: "Viewers",
                column: "StoryId",
                principalTable: "Stories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Viewers_Stories_StoryId",
                table: "Viewers");

            migrationBuilder.DropIndex(
                name: "IX_Viewers_StoryId",
                table: "Viewers");

            migrationBuilder.DropColumn(
                name: "StoryId",
                table: "Viewers");

            migrationBuilder.CreateTable(
                name: "ViewerDto",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    StoryId = table.Column<int>(type: "integer", nullable: true),
                    UserId = table.Column<string>(type: "text", nullable: true),
                    UserName = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ViewerDto", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ViewerDto_Stories_StoryId",
                        column: x => x.StoryId,
                        principalTable: "Stories",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ViewerDto_StoryId",
                table: "ViewerDto",
                column: "StoryId");
        }
    }
}
