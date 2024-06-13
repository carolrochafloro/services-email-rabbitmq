using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FormContato.Migrations
{
    /// <inheritdoc />
    public partial class Fix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Recipients_Users_AppUserId",
                table: "Recipients");

            migrationBuilder.DropIndex(
                name: "IX_Recipients_AppUserId",
                table: "Recipients");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "Recipients");

            migrationBuilder.CreateIndex(
                name: "IX_Recipients_UserId",
                table: "Recipients",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Recipients_Users_UserId",
                table: "Recipients",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Recipients_Users_UserId",
                table: "Recipients");

            migrationBuilder.DropIndex(
                name: "IX_Recipients_UserId",
                table: "Recipients");

            migrationBuilder.AddColumn<Guid>(
                name: "AppUserId",
                table: "Recipients",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Recipients_AppUserId",
                table: "Recipients",
                column: "AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Recipients_Users_AppUserId",
                table: "Recipients",
                column: "AppUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
