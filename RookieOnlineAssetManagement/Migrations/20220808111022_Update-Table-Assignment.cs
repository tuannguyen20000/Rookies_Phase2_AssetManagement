using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RookieOnlineAssetManagement.Migrations
{
    public partial class UpdateTableAssignment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AcceptedById",
                table: "Assignments",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RequestedById",
                table: "Assignments",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Assignments_AcceptedById",
                table: "Assignments",
                column: "AcceptedById");

            migrationBuilder.CreateIndex(
                name: "IX_Assignments_RequestedById",
                table: "Assignments",
                column: "RequestedById");

            migrationBuilder.AddForeignKey(
                name: "FK_Assignments_AspNetUsers_AcceptedById",
                table: "Assignments",
                column: "AcceptedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Assignments_AspNetUsers_RequestedById",
                table: "Assignments",
                column: "RequestedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assignments_AspNetUsers_AcceptedById",
                table: "Assignments");

            migrationBuilder.DropForeignKey(
                name: "FK_Assignments_AspNetUsers_RequestedById",
                table: "Assignments");

            migrationBuilder.DropIndex(
                name: "IX_Assignments_AcceptedById",
                table: "Assignments");

            migrationBuilder.DropIndex(
                name: "IX_Assignments_RequestedById",
                table: "Assignments");

            migrationBuilder.DropColumn(
                name: "AcceptedById",
                table: "Assignments");

            migrationBuilder.DropColumn(
                name: "RequestedById",
                table: "Assignments");
        }
    }
}
