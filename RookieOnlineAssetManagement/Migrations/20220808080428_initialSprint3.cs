using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RookieOnlineAssetManagement.Migrations
{
    public partial class initialSprint3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RequestState",
                table: "Assignments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "ReturnedDate",
                table: "Assignments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RequestState",
                table: "Assignments");

            migrationBuilder.DropColumn(
                name: "ReturnedDate",
                table: "Assignments");
        }
    }
}
