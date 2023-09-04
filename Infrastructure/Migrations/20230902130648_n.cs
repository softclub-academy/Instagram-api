using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class n : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserProfiles_Locations_LocationId",
                table: "UserProfiles");

            migrationBuilder.AlterColumn<int>(
                name: "LocationId",
                table: "UserProfiles",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_UserProfiles_Locations_LocationId",
                table: "UserProfiles",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "LocationId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserProfiles_Locations_LocationId",
                table: "UserProfiles");

            migrationBuilder.AlterColumn<int>(
                name: "LocationId",
                table: "UserProfiles",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_UserProfiles_Locations_LocationId",
                table: "UserProfiles",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "LocationId");
        }
    }
}
