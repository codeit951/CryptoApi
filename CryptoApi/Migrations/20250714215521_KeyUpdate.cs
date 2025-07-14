using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CryptoApi.Migrations
{
    /// <inheritdoc />
    public partial class KeyUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CryptoData_Symbol",
                table: "CryptoData");

            migrationBuilder.CreateTable(
                name: "CryptoKey",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Key = table.Column<string>(type: "TEXT", nullable: false),
                    DataId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CryptoKey", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CryptoKey_CryptoData_DataId",
                        column: x => x.DataId,
                        principalTable: "CryptoData",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CryptoKey_DataId",
                table: "CryptoKey",
                column: "DataId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CryptoKey");

            migrationBuilder.CreateIndex(
                name: "IX_CryptoData_Symbol",
                table: "CryptoData",
                column: "Symbol",
                unique: true);
        }
    }
}
