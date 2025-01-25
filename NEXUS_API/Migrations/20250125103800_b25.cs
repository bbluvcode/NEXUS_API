using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NEXUS_API.Migrations
{
    /// <inheritdoc />
    public partial class b25 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "isUsing",
                table: "Plans",
                newName: "IsUsing");

            migrationBuilder.AlterColumn<decimal>(
                name: "Deposit",
                table: "CustomerRequests",
                type: "decimal(10,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,2)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DepositStatus",
                table: "CustomerRequests",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DepositStatus",
                table: "CustomerRequests");

            migrationBuilder.RenameColumn(
                name: "IsUsing",
                table: "Plans",
                newName: "isUsing");

            migrationBuilder.AlterColumn<decimal>(
                name: "Deposit",
                table: "CustomerRequests",
                type: "decimal(10,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,2)");
        }
    }
}
