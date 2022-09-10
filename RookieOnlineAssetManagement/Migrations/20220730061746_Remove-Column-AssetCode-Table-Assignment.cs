using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RookieOnlineAssetManagement.Migrations
{
    public partial class RemoveColumnAssetCodeTableAssignment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AssetCode",
                table: "Assignments");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AssetCode",
                table: "Assignments",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
