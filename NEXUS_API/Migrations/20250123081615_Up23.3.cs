using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NEXUS_API.Migrations
{
    /// <inheritdoc />
    public partial class Up233 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Connections_Equipments_EquipmentId",
                table: "Connections");

            migrationBuilder.AlterColumn<int>(
                name: "EquipmentId",
                table: "Connections",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateTable(
                name: "InstallationOrders",
                columns: table => new
                {
                    InstallationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ServiceOrderId = table.Column<string>(type: "nvarchar(11)", nullable: false),
                    TechnicianId = table.Column<int>(type: "int", nullable: false),
                    DateAssigned = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateCompleted = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InstallationOrders", x => x.InstallationId);
                    table.ForeignKey(
                        name: "FK_InstallationOrders_Employees_TechnicianId",
                        column: x => x.TechnicianId,
                        principalTable: "Employees",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InstallationOrders_ServiceOrders_ServiceOrderId",
                        column: x => x.ServiceOrderId,
                        principalTable: "ServiceOrders",
                        principalColumn: "OrderId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InstallationOrders_ServiceOrderId",
                table: "InstallationOrders",
                column: "ServiceOrderId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_InstallationOrders_TechnicianId",
                table: "InstallationOrders",
                column: "TechnicianId");

            migrationBuilder.AddForeignKey(
                name: "FK_Connections_Equipments_EquipmentId",
                table: "Connections",
                column: "EquipmentId",
                principalTable: "Equipments",
                principalColumn: "EquipmentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Connections_Equipments_EquipmentId",
                table: "Connections");

            migrationBuilder.DropTable(
                name: "InstallationOrders");

            migrationBuilder.AlterColumn<int>(
                name: "EquipmentId",
                table: "Connections",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Connections_Equipments_EquipmentId",
                table: "Connections",
                column: "EquipmentId",
                principalTable: "Equipments",
                principalColumn: "EquipmentId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
