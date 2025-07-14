using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CryptoApi.Migrations
{
    /// <inheritdoc />
    public partial class keychange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_CryptoData_Symbol",
                table: "CryptoData",
                column: "Symbol",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CryptoData_Symbol",
                table: "CryptoData");
        }
    }
}
