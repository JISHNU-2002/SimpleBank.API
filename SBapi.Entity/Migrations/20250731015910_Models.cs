using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SBapi.Entity.Migrations
{
    /// <inheritdoc />
    public partial class Models : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccountType",
                table: "tblApplicationForm");

            migrationBuilder.DropColumn(
                name: "BranchName",
                table: "tblApplicationForm");

            migrationBuilder.DropColumn(
                name: "RegisteredAt",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<string>(
                name: "IFSC",
                table: "tblBranch",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldDefaultValueSql: "NEXT VALUE FOR dbo.IFSCSequence");

            migrationBuilder.AddColumn<int>(
                name: "AccountTypeId",
                table: "tblApplicationForm",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IFSC",
                table: "tblApplicationForm",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "CustomerId",
                table: "tblAccount",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "0ca18703-c5ca-41bb-a1bb-a0a7665aa8b9",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "9db18c3a-4342-4782-a9d1-9b1dfff70f23", "AQAAAAIAAYagAAAAEA1bsiNFDDzrkSEt9Ssb+6/E6GXgvH0OpCz32NrZs+jDVBbUN4Ejh4gg9sc9dYx55Q==", "3097cd66-17c5-473d-b342-41c9626d5545" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccountTypeId",
                table: "tblApplicationForm");

            migrationBuilder.DropColumn(
                name: "IFSC",
                table: "tblApplicationForm");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "tblAccount");

            migrationBuilder.AlterColumn<string>(
                name: "IFSC",
                table: "tblBranch",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValueSql: "NEXT VALUE FOR dbo.IFSCSequence",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AddColumn<string>(
                name: "AccountType",
                table: "tblApplicationForm",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "BranchName",
                table: "tblApplicationForm",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "RegisteredAt",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "0ca18703-c5ca-41bb-a1bb-a0a7665aa8b9",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "RegisteredAt", "SecurityStamp" },
                values: new object[] { "3cb0e46d-af6e-4696-9b18-92bf741956da", "AQAAAAIAAYagAAAAEPHeJDiZLDyzPWM9XTrkToRTBV/+zIh9YFrX5hbKNBrr8kSGS3m8QUmxxqILNpjMQw==", new DateTime(2025, 7, 30, 18, 0, 14, 200, DateTimeKind.Utc).AddTicks(8703), "a25b6b66-782b-4c67-9167-7e7e515b9fbd" });
        }
    }
}
