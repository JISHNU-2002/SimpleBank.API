using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SBapi.Entity.Migrations
{
    /// <inheritdoc />
    public partial class Transactions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tblTransaction",
                columns: table => new
                {
                    TransactionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FromAccountNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ToAccountNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TransactionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TransactionType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblTransaction", x => x.TransactionId);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "0ca18703-c5ca-41bb-a1bb-a0a7665aa8b9",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "ba532b8c-1f91-482f-904b-e07b33618256", "AQAAAAIAAYagAAAAEJRYgmjVrZKM6HIVlS/6vVUxvKP7P6ddqzMs7l2fL25+Oza1meICoOA5EZBe6h8TQw==", "c3ff0ab1-b1c0-4731-ac87-0ce640d16118" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tblTransaction");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "0ca18703-c5ca-41bb-a1bb-a0a7665aa8b9",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "ed71be50-84e9-488d-9c85-e48c82a97dea", "AQAAAAIAAYagAAAAEETkMc/bXi3WkBLvxfBCpPPQJHPn9MmmgUjBWtS8/l6R3mWGnQ1vVSUjZDmnZCBYgA==", "19bbadef-2d27-4670-adac-29597b6fab61" });
        }
    }
}
