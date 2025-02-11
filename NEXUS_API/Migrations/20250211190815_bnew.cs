using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NEXUS_API.Migrations
{
    /// <inheritdoc />
    public partial class bnew : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CustomerName",
                table: "SupportRequests",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CustomerName",
                table: "SupportRequests");
        }
    }
}
