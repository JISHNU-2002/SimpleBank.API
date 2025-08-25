using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SBapi.Entity.Migrations
{
    /// <inheritdoc />
    public partial class SuperAdmin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "b1c2d3e4-f5g6-h7i8-j9k0-l1m2n3o4p5q6", null, "SuperAdmin", "SUPERADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AadhaarNumber", "AccessFailedCount", "Address", "ConcurrencyStamp", "DateOfBirth", "Email", "EmailConfirmed", "FullName", "Gender", "IsActive", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PAN", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "RegisteredAt", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "0ca18703-c5ca-41bb-a1bb-a0a7665aa8b9", null, 0, null, "a21caf4c-3f0a-4c64-ac56-c5b540133b0a", null, "superadmin@gmail.com", false, null, null, true, false, null, "SUPERADMIN@GMAIL.COM", "SUPERADMIN@GMAIL.COM", null, "AQAAAAIAAYagAAAAEMxMGHtcQRr3r04rMge4iyK5M58pSkkMMweCTZSsrzKCHCO6L6uAXZqw7KWOutLnxg==", null, false, new DateTime(2025, 7, 30, 15, 1, 49, 344, DateTimeKind.Utc).AddTicks(5268), "7f8263c4-db11-40c2-a3f0-a470fdf57b56", false, "superadmin@gmail.com" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "b1c2d3e4-f5g6-h7i8-j9k0-l1m2n3o4p5q6", "0ca18703-c5ca-41bb-a1bb-a0a7665aa8b9" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "b1c2d3e4-f5g6-h7i8-j9k0-l1m2n3o4p5q6", "0ca18703-c5ca-41bb-a1bb-a0a7665aa8b9" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b1c2d3e4-f5g6-h7i8-j9k0-l1m2n3o4p5q6");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "0ca18703-c5ca-41bb-a1bb-a0a7665aa8b9");
        }
    }
}
