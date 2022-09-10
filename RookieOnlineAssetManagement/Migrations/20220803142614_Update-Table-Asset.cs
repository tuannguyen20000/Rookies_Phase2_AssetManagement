using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RookieOnlineAssetManagement.Migrations
{
    public partial class UpdateTableAsset : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CategoryPrefix",
                table: "Categories",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Disabled",
                table: "Assets",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Location",
                table: "Assets",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CategoryPrefix",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "Disabled",
                table: "Assets");

            migrationBuilder.DropColumn(
                name: "Location",
                table: "Assets");
        }
    }
}
