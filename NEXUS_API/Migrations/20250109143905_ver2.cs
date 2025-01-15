using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NEXUS_API.Migrations
{
    /// <inheritdoc />
    public partial class ver2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_EmployeeRole_EmployeeRoleId",
                table: "Employees");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EmployeeRole",
                table: "EmployeeRole");

            migrationBuilder.RenameTable(
                name: "EmployeeRole",
                newName: "EmployeeRoles");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EmployeeRoles",
                table: "EmployeeRoles",
                column: "RoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_EmployeeRoles_EmployeeRoleId",
                table: "Employees",
                column: "EmployeeRoleId",
                principalTable: "EmployeeRoles",
                principalColumn: "RoleId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_EmployeeRoles_EmployeeRoleId",
                table: "Employees");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EmployeeRoles",
                table: "EmployeeRoles");

            migrationBuilder.RenameTable(
                name: "EmployeeRoles",
                newName: "EmployeeRole");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EmployeeRole",
                table: "EmployeeRole",
                column: "RoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_EmployeeRole_EmployeeRoleId",
                table: "Employees",
                column: "EmployeeRoleId",
                principalTable: "EmployeeRole",
                principalColumn: "RoleId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
