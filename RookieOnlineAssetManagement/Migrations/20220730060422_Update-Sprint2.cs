using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RookieOnlineAssetManagement.Migrations
{
    public partial class UpdateSprint2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assignments_AssetInAssignments_AssetAssignmentId",
                table: "Assignments");

            migrationBuilder.RenameColumn(
                name: "AssetAssignmentId",
                table: "Assignments",
                newName: "AssetId");

            migrationBuilder.RenameIndex(
                name: "IX_Assignments_AssetAssignmentId",
                table: "Assignments",
                newName: "IX_Assignments_AssetId");

            migrationBuilder.AddColumn<string>(
                name: "AdminId",
                table: "Assignments",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Assignments",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Assignments_AdminId",
                table: "Assignments",
                column: "AdminId");

            migrationBuilder.CreateIndex(
                name: "IX_Assignments_UserId",
                table: "Assignments",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Assignments_AspNetUsers_AdminId",
                table: "Assignments",
                column: "AdminId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Assignments_AspNetUsers_UserId",
                table: "Assignments",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Assignments_Assets_AssetId",
                table: "Assignments",
                column: "AssetId",
                principalTable: "Assets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assignments_AspNetUsers_AdminId",
                table: "Assignments");

            migrationBuilder.DropForeignKey(
                name: "FK_Assignments_AspNetUsers_UserId",
                table: "Assignments");

            migrationBuilder.DropForeignKey(
                name: "FK_Assignments_Assets_AssetId",
                table: "Assignments");

            migrationBuilder.DropIndex(
                name: "IX_Assignments_AdminId",
                table: "Assignments");

            migrationBuilder.DropIndex(
                name: "IX_Assignments_UserId",
                table: "Assignments");

            migrationBuilder.DropColumn(
                name: "AdminId",
                table: "Assignments");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Assignments");

            migrationBuilder.RenameColumn(
                name: "AssetId",
                table: "Assignments",
                newName: "AssetAssignmentId");

            migrationBuilder.RenameIndex(
                name: "IX_Assignments_AssetId",
                table: "Assignments",
                newName: "IX_Assignments_AssetAssignmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Assignments_AssetInAssignments_AssetAssignmentId",
                table: "Assignments",
                column: "AssetAssignmentId",
                principalTable: "AssetInAssignments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
