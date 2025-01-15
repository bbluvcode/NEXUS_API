using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NEXUS_API.Migrations
{
    /// <inheritdoc />
    public partial class ver9 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Latitude",
                table: "Regions",
                type: "decimal(10,5)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Longitude",
                table: "Regions",
                type: "decimal(10,5)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "Regions");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "Regions");
        }
    }
}
