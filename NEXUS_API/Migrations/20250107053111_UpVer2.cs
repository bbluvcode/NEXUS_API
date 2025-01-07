using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NEXUS_API.Migrations
{
    /// <inheritdoc />
    public partial class UpVer2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ConnectionDiaries_Connections_ConnectionId",
                table: "ConnectionDiaries");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_EmployeeRoles_EmployeeRoleId",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_SupportRequests_Employees_EmployeeId",
                table: "SupportRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_SupportRequests_ServiceOrders_ServiceOrderId",
                table: "SupportRequests");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EmployeeRoles",
                table: "EmployeeRoles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ConnectionDiaries",
                table: "ConnectionDiaries");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "FeedBacks");

            migrationBuilder.RenameTable(
                name: "EmployeeRoles",
                newName: "EmployeeRole");

            migrationBuilder.RenameTable(
                name: "ConnectionDiaries",
                newName: "ConnectionDiary");

            migrationBuilder.RenameColumn(
                name: "ServiceOrderId",
                table: "SupportRequests",
                newName: "CustomerId");

            migrationBuilder.RenameColumn(
                name: "EmployeeId",
                table: "SupportRequests",
                newName: "ServiceOrderOrderId");

            migrationBuilder.RenameIndex(
                name: "IX_SupportRequests_ServiceOrderId",
                table: "SupportRequests",
                newName: "IX_SupportRequests_CustomerId");

            migrationBuilder.RenameIndex(
                name: "IX_SupportRequests_EmployeeId",
                table: "SupportRequests",
                newName: "IX_SupportRequests_ServiceOrderOrderId");

            migrationBuilder.RenameColumn(
                name: "RetainShopName",
                table: "RetailShops",
                newName: "RetailShopName");

            migrationBuilder.RenameIndex(
                name: "IX_ConnectionDiaries_ConnectionId",
                table: "ConnectionDiary",
                newName: "IX_ConnectionDiary_ConnectionId");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateResolved",
                table: "SupportRequests",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EmpIdResolver",
                table: "SupportRequests",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EmpIDCreater",
                table: "ServiceOrders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "InStockOrderDetails",
                type: "decimal(10,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddPrimaryKey(
                name: "PK_EmployeeRole",
                table: "EmployeeRole",
                column: "RoleId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ConnectionDiary",
                table: "ConnectionDiary",
                column: "DiaryId");

            migrationBuilder.CreateIndex(
                name: "IX_SupportRequests_EmpIdResolver",
                table: "SupportRequests",
                column: "EmpIdResolver");

            migrationBuilder.CreateIndex(
                name: "IX_SupportRequests_SupportRequestId",
                table: "SupportRequests",
                column: "SupportRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceOrders_EmpIDCreater",
                table: "ServiceOrders",
                column: "EmpIDCreater");

            migrationBuilder.AddForeignKey(
                name: "FK_ConnectionDiary_Connections_ConnectionId",
                table: "ConnectionDiary",
                column: "ConnectionId",
                principalTable: "Connections",
                principalColumn: "ConnectionId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_EmployeeRole_EmployeeRoleId",
                table: "Employees",
                column: "EmployeeRoleId",
                principalTable: "EmployeeRole",
                principalColumn: "RoleId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceOrders_Employees_EmpIDCreater",
                table: "ServiceOrders",
                column: "EmpIDCreater",
                principalTable: "Employees",
                principalColumn: "EmployeeId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SupportRequests_Customers_CustomerId",
                table: "SupportRequests",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "CustomerId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SupportRequests_Employees_EmpIdResolver",
                table: "SupportRequests",
                column: "EmpIdResolver",
                principalTable: "Employees",
                principalColumn: "EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_SupportRequests_ServiceOrders_ServiceOrderOrderId",
                table: "SupportRequests",
                column: "ServiceOrderOrderId",
                principalTable: "ServiceOrders",
                principalColumn: "OrderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ConnectionDiary_Connections_ConnectionId",
                table: "ConnectionDiary");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_EmployeeRole_EmployeeRoleId",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceOrders_Employees_EmpIDCreater",
                table: "ServiceOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_SupportRequests_Customers_CustomerId",
                table: "SupportRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_SupportRequests_Employees_EmpIdResolver",
                table: "SupportRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_SupportRequests_ServiceOrders_ServiceOrderOrderId",
                table: "SupportRequests");

            migrationBuilder.DropIndex(
                name: "IX_SupportRequests_EmpIdResolver",
                table: "SupportRequests");

            migrationBuilder.DropIndex(
                name: "IX_SupportRequests_SupportRequestId",
                table: "SupportRequests");

            migrationBuilder.DropIndex(
                name: "IX_ServiceOrders_EmpIDCreater",
                table: "ServiceOrders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EmployeeRole",
                table: "EmployeeRole");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ConnectionDiary",
                table: "ConnectionDiary");

            migrationBuilder.DropColumn(
                name: "DateResolved",
                table: "SupportRequests");

            migrationBuilder.DropColumn(
                name: "EmpIdResolver",
                table: "SupportRequests");

            migrationBuilder.DropColumn(
                name: "EmpIDCreater",
                table: "ServiceOrders");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "InStockOrderDetails");

            migrationBuilder.RenameTable(
                name: "EmployeeRole",
                newName: "EmployeeRoles");

            migrationBuilder.RenameTable(
                name: "ConnectionDiary",
                newName: "ConnectionDiaries");

            migrationBuilder.RenameColumn(
                name: "ServiceOrderOrderId",
                table: "SupportRequests",
                newName: "EmployeeId");

            migrationBuilder.RenameColumn(
                name: "CustomerId",
                table: "SupportRequests",
                newName: "ServiceOrderId");

            migrationBuilder.RenameIndex(
                name: "IX_SupportRequests_ServiceOrderOrderId",
                table: "SupportRequests",
                newName: "IX_SupportRequests_EmployeeId");

            migrationBuilder.RenameIndex(
                name: "IX_SupportRequests_CustomerId",
                table: "SupportRequests",
                newName: "IX_SupportRequests_ServiceOrderId");

            migrationBuilder.RenameColumn(
                name: "RetailShopName",
                table: "RetailShops",
                newName: "RetainShopName");

            migrationBuilder.RenameIndex(
                name: "IX_ConnectionDiary_ConnectionId",
                table: "ConnectionDiaries",
                newName: "IX_ConnectionDiaries_ConnectionId");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "FeedBacks",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EmployeeRoles",
                table: "EmployeeRoles",
                column: "RoleId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ConnectionDiaries",
                table: "ConnectionDiaries",
                column: "DiaryId");

            migrationBuilder.AddForeignKey(
                name: "FK_ConnectionDiaries_Connections_ConnectionId",
                table: "ConnectionDiaries",
                column: "ConnectionId",
                principalTable: "Connections",
                principalColumn: "ConnectionId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_EmployeeRoles_EmployeeRoleId",
                table: "Employees",
                column: "EmployeeRoleId",
                principalTable: "EmployeeRoles",
                principalColumn: "RoleId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SupportRequests_Employees_EmployeeId",
                table: "SupportRequests",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_SupportRequests_ServiceOrders_ServiceOrderId",
                table: "SupportRequests",
                column: "ServiceOrderId",
                principalTable: "ServiceOrders",
                principalColumn: "OrderId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
