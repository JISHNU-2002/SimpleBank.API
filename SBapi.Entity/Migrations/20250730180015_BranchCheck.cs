using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SBapi.Entity.Migrations
{
    /// <inheritdoc />
    public partial class BranchCheck : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AadhaarNumber",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "DateOfBirth",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "FullName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Gender",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PAN",
                table: "AspNetUsers");

            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.CreateSequence<int>(
                name: "IFSCSequence",
                schema: "dbo",
                startValue: 5993L);

            migrationBuilder.AddColumn<string>(
                name: "AccountNumber",
                table: "AspNetUsers",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FormId",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "tblAccount",
                columns: table => new
                {
                    AccountNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Balance = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblAccount", x => x.AccountNumber);
                });

            migrationBuilder.CreateTable(
                name: "tblAccountType",
                columns: table => new
                {
                    TypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TypeName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MinBalance = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblAccountType", x => x.TypeId);
                });

            migrationBuilder.CreateTable(
                name: "tblApplicationForm",
                columns: table => new
                {
                    FormId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AadharNumber = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: false),
                    PAN = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DOB = table.Column<DateTime>(type: "datetime2", maxLength: 20, nullable: false),
                    AccountType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BranchName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    DateOfRegistration = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblApplicationForm", x => x.FormId);
                });

            migrationBuilder.CreateTable(
                name: "tblBranch",
                columns: table => new
                {
                    IFSC = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, defaultValueSql: "NEXT VALUE FOR dbo.IFSCSequence"),
                    BranchName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    State = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Country = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblBranch", x => x.IFSC);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "0ca18703-c5ca-41bb-a1bb-a0a7665aa8b9",
                columns: new[] { "AccountNumber", "ConcurrencyStamp", "FormId", "PasswordHash", "RegisteredAt", "SecurityStamp" },
                values: new object[] { null, "3cb0e46d-af6e-4696-9b18-92bf741956da", 0, "AQAAAAIAAYagAAAAEPHeJDiZLDyzPWM9XTrkToRTBV/+zIh9YFrX5hbKNBrr8kSGS3m8QUmxxqILNpjMQw==", new DateTime(2025, 7, 30, 18, 0, 14, 200, DateTimeKind.Utc).AddTicks(8703), "a25b6b66-782b-4c67-9167-7e7e515b9fbd" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tblAccount");

            migrationBuilder.DropTable(
                name: "tblAccountType");

            migrationBuilder.DropTable(
                name: "tblApplicationForm");

            migrationBuilder.DropTable(
                name: "tblBranch");

            migrationBuilder.DropColumn(
                name: "AccountNumber",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "FormId",
                table: "AspNetUsers");

            migrationBuilder.DropSequence(
                name: "IFSCSequence",
                schema: "dbo");

            migrationBuilder.AddColumn<string>(
                name: "AadhaarNumber",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfBirth",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FullName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Gender",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PAN",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "0ca18703-c5ca-41bb-a1bb-a0a7665aa8b9",
                columns: new[] { "AadhaarNumber", "Address", "ConcurrencyStamp", "DateOfBirth", "FullName", "Gender", "PAN", "PasswordHash", "RegisteredAt", "SecurityStamp" },
                values: new object[] { null, null, "a21caf4c-3f0a-4c64-ac56-c5b540133b0a", null, null, null, null, "AQAAAAIAAYagAAAAEMxMGHtcQRr3r04rMge4iyK5M58pSkkMMweCTZSsrzKCHCO6L6uAXZqw7KWOutLnxg==", new DateTime(2025, 7, 30, 15, 1, 49, 344, DateTimeKind.Utc).AddTicks(5268), "7f8263c4-db11-40c2-a3f0-a470fdf57b56" });
        }
    }
}
