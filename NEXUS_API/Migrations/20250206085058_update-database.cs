using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NEXUS_API.Migrations
{
    /// <inheritdoc />
    public partial class updatedatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Stocks_Email",
                table: "Stocks",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Stocks_Fax",
                table: "Stocks",
                column: "Fax",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Stocks_Phone",
                table: "Stocks",
                column: "Phone",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Stocks_StockName",
                table: "Stocks",
                column: "StockName",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Stocks_Email",
                table: "Stocks");

            migrationBuilder.DropIndex(
                name: "IX_Stocks_Fax",
                table: "Stocks");

            migrationBuilder.DropIndex(
                name: "IX_Stocks_Phone",
                table: "Stocks");

            migrationBuilder.DropIndex(
                name: "IX_Stocks_StockName",
                table: "Stocks");
        }
    }
}
