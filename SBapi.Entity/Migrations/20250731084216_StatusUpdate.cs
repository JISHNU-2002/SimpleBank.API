using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SBapi.Entity.Migrations
{
    /// <inheritdoc />
    public partial class StatusUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "IFSC",
                table: "tblApplicationForm",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

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
                values: new object[] { "0b38cd9c-28e1-4b39-aacd-6a29178d9f1f", "AQAAAAIAAYagAAAAEIQX/NLQM7BN6rYe7pSg4Lg7a0/shGgpchKkD4IBs40K8CJ5QvI13Pzyy7IH3D/oUQ==", "78de173d-b32f-417e-a8a7-07234c1b7f71" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IFSC",
                table: "tblAccount");

            migrationBuilder.AlterColumn<int>(
                name: "IFSC",
                table: "tblApplicationForm",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "0ca18703-c5ca-41bb-a1bb-a0a7665aa8b9",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "51050773-1db0-4f07-b71a-457f5ed54b41", "AQAAAAIAAYagAAAAEEjmb/tqWAvvgSdMGaVkZcBUXR7vivzd88NU5ZN3pqzEk0BzweE4Da+YogmUTN8UUg==", "f35928b9-5f30-4396-834e-186a7e3c6a2d" });
        }
    }
}
