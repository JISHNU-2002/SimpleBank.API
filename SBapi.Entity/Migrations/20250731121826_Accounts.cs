using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SBapi.Entity.Migrations
{
    /// <inheritdoc />
    public partial class Accounts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IFSC",
                table: "tblAccount");

            migrationBuilder.CreateSequence<int>(
                name: "AccountNumberSequence",
                schema: "dbo",
                startValue: 11235813L);

            migrationBuilder.AlterColumn<string>(
                name: "AccountNumber",
                table: "tblAccount",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "0ca18703-c5ca-41bb-a1bb-a0a7665aa8b9",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "ed71be50-84e9-488d-9c85-e48c82a97dea", "AQAAAAIAAYagAAAAEETkMc/bXi3WkBLvxfBCpPPQJHPn9MmmgUjBWtS8/l6R3mWGnQ1vVSUjZDmnZCBYgA==", "19bbadef-2d27-4670-adac-29597b6fab61" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropSequence(
                name: "AccountNumberSequence",
                schema: "dbo");

            migrationBuilder.AlterColumn<string>(
                name: "AccountNumber",
                table: "tblAccount",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AddColumn<string>(
                name: "IFSC",
                table: "tblAccount",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "0ca18703-c5ca-41bb-a1bb-a0a7665aa8b9",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "d764cfa1-2e17-415b-87d5-fa62df1d4a8f", "AQAAAAIAAYagAAAAENzBPdWekWDyPzgQJQY+Ni0n68kIh13n7rSL43mFoFbr01L/QLxIMY6q+g/u8JFU/A==", "2f0616d3-c7ae-42e8-9519-79ef763ede51" });
        }
    }
}
