using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class init2321 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Images_UserProfiles_UserProfileId",
                table: "Images");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserProfiles",
                table: "UserProfiles");

            migrationBuilder.DropIndex(
                name: "IX_Images_UserProfileId",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "UserProfileId",
                table: "UserProfiles");

            migrationBuilder.AddColumn<string>(
                name: "UserProfileUserId",
                table: "Images",
                type: "text",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserProfiles",
                table: "UserProfiles",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Images_UserProfileUserId",
                table: "Images",
                column: "UserProfileUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Images_UserProfiles_UserProfileUserId",
                table: "Images",
                column: "UserProfileUserId",
                principalTable: "UserProfiles",
                principalColumn: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Images_UserProfiles_UserProfileUserId",
                table: "Images");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserProfiles",
                table: "UserProfiles");

            migrationBuilder.DropIndex(
                name: "IX_Images_UserProfileUserId",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "UserProfileUserId",
                table: "Images");

            migrationBuilder.AddColumn<int>(
                name: "UserProfileId",
                table: "UserProfiles",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserProfiles",
                table: "UserProfiles",
                column: "UserProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_Images_UserProfileId",
                table: "Images",
                column: "UserProfileId");

            migrationBuilder.AddForeignKey(
                name: "FK_Images_UserProfiles_UserProfileId",
                table: "Images",
                column: "UserProfileId",
                principalTable: "UserProfiles",
                principalColumn: "UserProfileId");
        }
    }
}
