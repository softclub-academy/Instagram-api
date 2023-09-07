using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class v8 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Images",
                table: "Stories");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreateAt",
                table: "Stories",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "FileName",
                table: "Stories",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Stories",
                type: "text",
                nullable: false,
                defaultValue: "");

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Stories_AspNetUsers_UserId",
                table: "Stories");

            migrationBuilder.DropIndex(
                name: "IX_Stories_UserId",
                table: "Stories");

            migrationBuilder.DropColumn(
                name: "CreateAt",
                table: "Stories");

            migrationBuilder.DropColumn(
                name: "FileName",
                table: "Stories");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Stories");

            migrationBuilder.AddColumn<List<string>>(
                name: "Images",
                table: "Stories",
                type: "text[]",
                nullable: false);
        }
    }
}
