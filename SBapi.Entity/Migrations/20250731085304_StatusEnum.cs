using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SBapi.Entity.Migrations
{
    /// <inheritdoc />
    public partial class StatusEnum : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "0ca18703-c5ca-41bb-a1bb-a0a7665aa8b9",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "d764cfa1-2e17-415b-87d5-fa62df1d4a8f", "AQAAAAIAAYagAAAAENzBPdWekWDyPzgQJQY+Ni0n68kIh13n7rSL43mFoFbr01L/QLxIMY6q+g/u8JFU/A==", "2f0616d3-c7ae-42e8-9519-79ef763ede51" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "0ca18703-c5ca-41bb-a1bb-a0a7665aa8b9",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "0b38cd9c-28e1-4b39-aacd-6a29178d9f1f", "AQAAAAIAAYagAAAAEIQX/NLQM7BN6rYe7pSg4Lg7a0/shGgpchKkD4IBs40K8CJ5QvI13Pzyy7IH3D/oUQ==", "78de173d-b32f-417e-a8a7-07234c1b7f71" });
        }
    }
}
