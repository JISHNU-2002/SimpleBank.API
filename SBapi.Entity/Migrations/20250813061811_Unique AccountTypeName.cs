using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SBapi.Entity.Migrations
{
    /// <inheritdoc />
    public partial class UniqueAccountTypeName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "0ca18703-c5ca-41bb-a1bb-a0a7665aa8b9",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "0d9fbd88-3436-4c98-9f6d-b536772bb797", "AQAAAAIAAYagAAAAEJA3ELHStPM1l6mwLC1r4c3c4bcAEQTEXS7oaU8hh/802LfIDyMBuKocgqcjy8U8+Q==", "355ef620-343b-4ce6-86a8-8b9af4fbdc1b" });

            migrationBuilder.CreateIndex(
                name: "IX_tblAccountType_TypeName",
                table: "tblAccountType",
                column: "TypeName",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_tblAccountType_TypeName",
                table: "tblAccountType");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "0ca18703-c5ca-41bb-a1bb-a0a7665aa8b9",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "ba532b8c-1f91-482f-904b-e07b33618256", "AQAAAAIAAYagAAAAEJRYgmjVrZKM6HIVlS/6vVUxvKP7P6ddqzMs7l2fL25+Oza1meICoOA5EZBe6h8TQw==", "c3ff0ab1-b1c0-4731-ac87-0ce640d16118" });
        }
    }
}
