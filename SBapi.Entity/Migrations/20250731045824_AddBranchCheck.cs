using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SBapi.Entity.Migrations
{
    /// <inheritdoc />
    public partial class AddBranchCheck : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "tblAccount");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "0ca18703-c5ca-41bb-a1bb-a0a7665aa8b9",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "51050773-1db0-4f07-b71a-457f5ed54b41", "AQAAAAIAAYagAAAAEEjmb/tqWAvvgSdMGaVkZcBUXR7vivzd88NU5ZN3pqzEk0BzweE4Da+YogmUTN8UUg==", "f35928b9-5f30-4396-834e-186a7e3c6a2d" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
    }
}
