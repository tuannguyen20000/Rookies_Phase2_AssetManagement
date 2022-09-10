using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RookieOnlineAssetManagement.Migrations
{
    public partial class UpdateRecord : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f2fbe512-c499-4791-80fb-2b8a6d76fc57");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "46b2dcd0-f84a-4200-8d39-4e9b4667691b", "3aadfd1f-2a6e-42ac-af77-d7d1faad3b39" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "46b2dcd0-f84a-4200-8d39-4e9b4667691b");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "3aadfd1f-2a6e-42ac-af77-d7d1faad3b39");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "46b2dcd0-f84a-4200-8d39-4e9b4667691b", "41a32c80-842c-49ae-a593-612b92f74a12", "Admin", "admin" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "f2fbe512-c499-4791-80fb-2b8a6d76fc57", "2280a320-ef31-4864-9802-e7a90f8181e9", "Staff", "staff" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "DateOfBirth", "Disabled", "Email", "EmailConfirmed", "FirstLogin", "FirstName", "Gender", "JoinedDate", "LastName", "Location", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "StaffCode", "TwoFactorEnabled", "UserName" },
                values: new object[] { "3aadfd1f-2a6e-42ac-af77-d7d1faad3b39", 0, "fa4a4d0b-773d-4f22-9c70-56379d322250", new DateTime(1999, 9, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "huycnh@gmail.com", true, true, "Huy", 0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Chau", 0, false, null, "huycnh@gmail.com", "admin", "AQAAAAEAACcQAAAAEK8j/Bl3bBHgt/z28L/yjxEhUvjiFVWrXuCb4Yrgb0gzlA6XsdCX15Hbi4vMnCEJaA==", null, false, "", "SD0001", false, "admin" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "46b2dcd0-f84a-4200-8d39-4e9b4667691b", "3aadfd1f-2a6e-42ac-af77-d7d1faad3b39" });
        }
    }
}
