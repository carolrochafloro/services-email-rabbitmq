using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FormContato.Migrations
{
    /// <inheritdoc />
    public partial class SentToContact : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ContactDTO");

            migrationBuilder.AddColumn<string>(
                name: "SentTo",
                table: "Contacts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SentTo",
                table: "Contacts");

            migrationBuilder.CreateTable(
                name: "ContactDTO",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactDTO", x => x.Id);
                });
        }
    }
}
