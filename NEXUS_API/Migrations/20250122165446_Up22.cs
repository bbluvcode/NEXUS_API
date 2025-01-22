using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NEXUS_API.Migrations
{
    /// <inheritdoc />
    public partial class Up22 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    CustomerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(7)", maxLength: 7, nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    IdentificationNo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Image = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RefreshToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RefreshTokenExpried = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FailedLoginAttempts = table.Column<int>(type: "int", nullable: false),
                    ExpiredBan = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExpiredCode = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SendCodeAttempts = table.Column<int>(type: "int", nullable: false),
                    LastSendCodeDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.CustomerId);
                });

            migrationBuilder.CreateTable(
                name: "Discounts",
                columns: table => new
                {
                    DiscountId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DiscountName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DiscountPercentage = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ApplyTo = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Discounts", x => x.DiscountId);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeRoles",
                columns: table => new
                {
                    RoleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeRoles", x => x.RoleId);
                });

            migrationBuilder.CreateTable(
                name: "EquipmentTypes",
                columns: table => new
                {
                    EquipmentTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TypeName = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Provider = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentTypes", x => x.EquipmentTypeId);
                });

            migrationBuilder.CreateTable(
                name: "Plans",
                columns: table => new
                {
                    PlanId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlanName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SecurityDeposit = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    isUsing = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Plans", x => x.PlanId);
                });

            migrationBuilder.CreateTable(
                name: "Regions",
                columns: table => new
                {
                    RegionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RegionCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    RegionName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Latitude = table.Column<decimal>(type: "decimal(10,5)", nullable: false),
                    Longitude = table.Column<decimal>(type: "decimal(10,5)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Regions", x => x.RegionId);
                });

            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    AccountId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.AccountId);
                    table.ForeignKey(
                        name: "FK_Accounts_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FeedBacks",
                columns: table => new
                {
                    FeedBackId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    FeedBackContent = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false),
                    CustomerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeedBacks", x => x.FeedBackId);
                    table.ForeignKey(
                        name: "FK_FeedBacks_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlanFees",
                columns: table => new
                {
                    PlanFeeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlanFeeName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    IsUsing = table.Column<bool>(type: "bit", nullable: false),
                    Rental = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    CallCharge = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    DTDCallCharge = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    MessageMobileCharge = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    LocalCallCharge = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    PlanId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlanFees", x => x.PlanFeeId);
                    table.ForeignKey(
                        name: "FK_PlanFees_Plans_PlanId",
                        column: x => x.PlanId,
                        principalTable: "Plans",
                        principalColumn: "PlanId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CustomerRequests",
                columns: table => new
                {
                    RequestId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RequestTitle = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ServiceRequest = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    EquipmentRequest = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateCreate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateResolve = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsResponse = table.Column<bool>(type: "bit", nullable: false),
                    RegionId = table.Column<int>(type: "int", nullable: false),
                    CustomerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerRequests", x => x.RequestId);
                    table.ForeignKey(
                        name: "FK_CustomerRequests_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CustomerRequests_Regions_RegionId",
                        column: x => x.RegionId,
                        principalTable: "Regions",
                        principalColumn: "RegionId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RetailShops",
                columns: table => new
                {
                    RetailShopId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RetailShopName = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    IsMainOffice = table.Column<bool>(type: "bit", nullable: false),
                    Fax = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    Image = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    RegionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RetailShops", x => x.RetailShopId);
                    table.ForeignKey(
                        name: "FK_RetailShops_Regions_RegionId",
                        column: x => x.RegionId,
                        principalTable: "Regions",
                        principalColumn: "RegionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Stocks",
                columns: table => new
                {
                    StockId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StockName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Fax = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    RegionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stocks", x => x.StockId);
                    table.ForeignKey(
                        name: "FK_Stocks_Regions_RegionId",
                        column: x => x.RegionId,
                        principalTable: "Regions",
                        principalColumn: "RegionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Vendors",
                columns: table => new
                {
                    VendorId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VendorName = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Fax = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    RegionId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vendors", x => x.VendorId);
                    table.ForeignKey(
                        name: "FK_Vendors_Regions_RegionId",
                        column: x => x.RegionId,
                        principalTable: "Regions",
                        principalColumn: "RegionId");
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    EmployeeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeCode = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(7)", maxLength: 7, nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    IdentificationNo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false),
                    RefreshToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RefreshTokenExpried = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FailedLoginAttempts = table.Column<int>(type: "int", nullable: false),
                    ExpiredBan = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExpiredCode = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SendCodeAttempts = table.Column<int>(type: "int", nullable: false),
                    LastSendCodeDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EmployeeRoleId = table.Column<int>(type: "int", nullable: false),
                    RetailShopId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.EmployeeId);
                    table.ForeignKey(
                        name: "FK_Employees_EmployeeRoles_EmployeeRoleId",
                        column: x => x.EmployeeRoleId,
                        principalTable: "EmployeeRoles",
                        principalColumn: "RoleId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Employees_RetailShops_RetailShopId",
                        column: x => x.RetailShopId,
                        principalTable: "RetailShops",
                        principalColumn: "RetailShopId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Equipments",
                columns: table => new
                {
                    EquipmentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EquipmentName = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    StockQuantity = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false),
                    DiscountId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    EquipmentTypeId = table.Column<int>(type: "int", nullable: false),
                    VendorId = table.Column<int>(type: "int", nullable: false),
                    StockId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Equipments", x => x.EquipmentId);
                    table.ForeignKey(
                        name: "FK_Equipments_Discounts_DiscountId",
                        column: x => x.DiscountId,
                        principalTable: "Discounts",
                        principalColumn: "DiscountId");
                    table.ForeignKey(
                        name: "FK_Equipments_EquipmentTypes_EquipmentTypeId",
                        column: x => x.EquipmentTypeId,
                        principalTable: "EquipmentTypes",
                        principalColumn: "EquipmentTypeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Equipments_Stocks_StockId",
                        column: x => x.StockId,
                        principalTable: "Stocks",
                        principalColumn: "StockId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Equipments_Vendors_VendorId",
                        column: x => x.VendorId,
                        principalTable: "Vendors",
                        principalColumn: "VendorId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InStockRequests",
                columns: table => new
                {
                    InStockRequestId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TotalNumber = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InStockRequests", x => x.InStockRequestId);
                    table.ForeignKey(
                        name: "FK_InStockRequests_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NewsTB",
                columns: table => new
                {
                    NewsId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    EmployeeId = table.Column<int>(type: "int", maxLength: 50, nullable: false),
                    Content = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NewsTB", x => x.NewsId);
                    table.ForeignKey(
                        name: "FK_NewsTB_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OutStockOrders",
                columns: table => new
                {
                    OutStockId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StockId = table.Column<int>(type: "int", nullable: false),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PayDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Tax = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Total = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    isPay = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OutStockOrders", x => x.OutStockId);
                    table.ForeignKey(
                        name: "FK_OutStockOrders_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OutStockOrders_Stocks_StockId",
                        column: x => x.StockId,
                        principalTable: "Stocks",
                        principalColumn: "StockId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ServiceOrders",
                columns: table => new
                {
                    OrderId = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: false),
                    DateCreate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Deposit = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    EmpIDCreater = table.Column<int>(type: "int", nullable: true),
                    EmpIDSurveyor = table.Column<int>(type: "int", nullable: true),
                    SurveyDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SurveyStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SurveyDescribe = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AccountId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    RequestId = table.Column<int>(type: "int", nullable: true),
                    PlanFeeId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceOrders", x => x.OrderId);
                    table.ForeignKey(
                        name: "FK_ServiceOrders_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_ServiceOrders_CustomerRequests_RequestId",
                        column: x => x.RequestId,
                        principalTable: "CustomerRequests",
                        principalColumn: "RequestId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ServiceOrders_Employees_EmpIDCreater",
                        column: x => x.EmpIDCreater,
                        principalTable: "Employees",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ServiceOrders_Employees_EmpIDSurveyor",
                        column: x => x.EmpIDSurveyor,
                        principalTable: "Employees",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ServiceOrders_PlanFees_PlanFeeId",
                        column: x => x.PlanFeeId,
                        principalTable: "PlanFees",
                        principalColumn: "PlanFeeId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SupportRequests",
                columns: table => new
                {
                    SupportRequestId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateRequest = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    DetailContent = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    DateResolved = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsResolved = table.Column<bool>(type: "bit", nullable: false),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    EmpIdResolver = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupportRequests", x => x.SupportRequestId);
                    table.ForeignKey(
                        name: "FK_SupportRequests_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SupportRequests_Employees_EmpIdResolver",
                        column: x => x.EmpIdResolver,
                        principalTable: "Employees",
                        principalColumn: "EmployeeId");
                });

            migrationBuilder.CreateTable(
                name: "InStockOrders",
                columns: table => new
                {
                    InStockOrderId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InStockRequestId = table.Column<int>(type: "int", nullable: false),
                    VendorId = table.Column<int>(type: "int", nullable: false),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    StockId = table.Column<int>(type: "int", nullable: false),
                    Payer = table.Column<int>(type: "int", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    InstockDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PayDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Tax = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Total = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    CurrencyUnit = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    isPay = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InStockOrders", x => x.InStockOrderId);
                    table.ForeignKey(
                        name: "FK_InStockOrders_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InStockOrders_InStockRequests_InStockRequestId",
                        column: x => x.InStockRequestId,
                        principalTable: "InStockRequests",
                        principalColumn: "InStockRequestId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InStockOrders_Stocks_StockId",
                        column: x => x.StockId,
                        principalTable: "Stocks",
                        principalColumn: "StockId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InStockOrders_Vendors_VendorId",
                        column: x => x.VendorId,
                        principalTable: "Vendors",
                        principalColumn: "VendorId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "InStockRequestDetails",
                columns: table => new
                {
                    InStockRequestDetailId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InStockRequestId = table.Column<int>(type: "int", nullable: false),
                    EquipmentId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InStockRequestDetails", x => x.InStockRequestDetailId);
                    table.ForeignKey(
                        name: "FK_InStockRequestDetails_Equipments_EquipmentId",
                        column: x => x.EquipmentId,
                        principalTable: "Equipments",
                        principalColumn: "EquipmentId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InStockRequestDetails_InStockRequests_InStockRequestId",
                        column: x => x.InStockRequestId,
                        principalTable: "InStockRequests",
                        principalColumn: "InStockRequestId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OutStockOrderDetails",
                columns: table => new
                {
                    OutStockDetailId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OutStockId = table.Column<int>(type: "int", nullable: false),
                    EquipmentId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(10,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OutStockOrderDetails", x => x.OutStockDetailId);
                    table.ForeignKey(
                        name: "FK_OutStockOrderDetails_Equipments_EquipmentId",
                        column: x => x.EquipmentId,
                        principalTable: "Equipments",
                        principalColumn: "EquipmentId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OutStockOrderDetails_OutStockOrders_OutStockId",
                        column: x => x.OutStockId,
                        principalTable: "OutStockOrders",
                        principalColumn: "OutStockId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Connections",
                columns: table => new
                {
                    ConnectionId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    NumberOfConnections = table.Column<int>(type: "int", nullable: false),
                    DateCreate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ServiceOrderId = table.Column<string>(type: "nvarchar(11)", nullable: false),
                    PlanId = table.Column<int>(type: "int", nullable: false),
                    EquipmentId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Connections", x => x.ConnectionId);
                    table.ForeignKey(
                        name: "FK_Connections_Equipments_EquipmentId",
                        column: x => x.EquipmentId,
                        principalTable: "Equipments",
                        principalColumn: "EquipmentId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Connections_Plans_PlanId",
                        column: x => x.PlanId,
                        principalTable: "Plans",
                        principalColumn: "PlanId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Connections_ServiceOrders_ServiceOrderId",
                        column: x => x.ServiceOrderId,
                        principalTable: "ServiceOrders",
                        principalColumn: "OrderId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ServiceBills",
                columns: table => new
                {
                    BillId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Payer = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FromDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ToDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PayDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Tax = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Total = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    IsPay = table.Column<bool>(type: "bit", nullable: false),
                    ServiceOrderId = table.Column<string>(type: "nvarchar(11)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceBills", x => x.BillId);
                    table.ForeignKey(
                        name: "FK_ServiceBills_ServiceOrders_ServiceOrderId",
                        column: x => x.ServiceOrderId,
                        principalTable: "ServiceOrders",
                        principalColumn: "OrderId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InStockOrderDetails",
                columns: table => new
                {
                    InStockOrderDetailId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    InStockOrderId = table.Column<int>(type: "int", nullable: false),
                    EquipmentId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InStockOrderDetails", x => x.InStockOrderDetailId);
                    table.ForeignKey(
                        name: "FK_InStockOrderDetails_Equipments_EquipmentId",
                        column: x => x.EquipmentId,
                        principalTable: "Equipments",
                        principalColumn: "EquipmentId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InStockOrderDetails_InStockOrders_InStockOrderId",
                        column: x => x.InStockOrderId,
                        principalTable: "InStockOrders",
                        principalColumn: "InStockOrderId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ConnectionDiary",
                columns: table => new
                {
                    DiaryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateStart = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateEnd = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ConnectionId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConnectionDiary", x => x.DiaryId);
                    table.ForeignKey(
                        name: "FK_ConnectionDiary_Connections_ConnectionId",
                        column: x => x.ConnectionId,
                        principalTable: "Connections",
                        principalColumn: "ConnectionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ServiceBillDetails",
                columns: table => new
                {
                    BillDetailId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Deposit = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Discount = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Rental = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    RentalDiscount = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    CallCharge = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    CallChargeTime = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    LocalCallCharge = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    LocalTime = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    STDCallCharge = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    STDTime = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    MessageMobileCharge = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    MessageMobileTime = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    ServiceDiscount = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    BillId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceBillDetails", x => x.BillDetailId);
                    table.ForeignKey(
                        name: "FK_ServiceBillDetails_ServiceBills_BillId",
                        column: x => x.BillId,
                        principalTable: "ServiceBills",
                        principalColumn: "BillId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_AccountId",
                table: "Accounts",
                column: "AccountId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_CustomerId",
                table: "Accounts",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_ConnectionDiary_ConnectionId",
                table: "ConnectionDiary",
                column: "ConnectionId");

            migrationBuilder.CreateIndex(
                name: "IX_Connections_EquipmentId",
                table: "Connections",
                column: "EquipmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Connections_PlanId",
                table: "Connections",
                column: "PlanId");

            migrationBuilder.CreateIndex(
                name: "IX_Connections_ServiceOrderId",
                table: "Connections",
                column: "ServiceOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerRequests_CustomerId",
                table: "CustomerRequests",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerRequests_RegionId",
                table: "CustomerRequests",
                column: "RegionId");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_Email",
                table: "Customers",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Customers_PhoneNumber",
                table: "Customers",
                column: "PhoneNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Employees_EmployeeRoleId",
                table: "Employees",
                column: "EmployeeRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_RetailShopId",
                table: "Employees",
                column: "RetailShopId");

            migrationBuilder.CreateIndex(
                name: "IX_Equipments_DiscountId",
                table: "Equipments",
                column: "DiscountId");

            migrationBuilder.CreateIndex(
                name: "IX_Equipments_EquipmentName",
                table: "Equipments",
                column: "EquipmentName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Equipments_EquipmentTypeId",
                table: "Equipments",
                column: "EquipmentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Equipments_StockId",
                table: "Equipments",
                column: "StockId");

            migrationBuilder.CreateIndex(
                name: "IX_Equipments_VendorId",
                table: "Equipments",
                column: "VendorId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentTypes_TypeName",
                table: "EquipmentTypes",
                column: "TypeName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FeedBacks_CustomerId",
                table: "FeedBacks",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_InStockOrderDetails_EquipmentId",
                table: "InStockOrderDetails",
                column: "EquipmentId");

            migrationBuilder.CreateIndex(
                name: "IX_InStockOrderDetails_InStockOrderId",
                table: "InStockOrderDetails",
                column: "InStockOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_InStockOrders_EmployeeId",
                table: "InStockOrders",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_InStockOrders_InStockRequestId",
                table: "InStockOrders",
                column: "InStockRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_InStockOrders_StockId",
                table: "InStockOrders",
                column: "StockId");

            migrationBuilder.CreateIndex(
                name: "IX_InStockOrders_VendorId",
                table: "InStockOrders",
                column: "VendorId");

            migrationBuilder.CreateIndex(
                name: "IX_InStockRequestDetails_EquipmentId",
                table: "InStockRequestDetails",
                column: "EquipmentId");

            migrationBuilder.CreateIndex(
                name: "IX_InStockRequestDetails_InStockRequestId",
                table: "InStockRequestDetails",
                column: "InStockRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_InStockRequests_EmployeeId",
                table: "InStockRequests",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_NewsTB_EmployeeId",
                table: "NewsTB",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_OutStockOrderDetails_EquipmentId",
                table: "OutStockOrderDetails",
                column: "EquipmentId");

            migrationBuilder.CreateIndex(
                name: "IX_OutStockOrderDetails_OutStockId",
                table: "OutStockOrderDetails",
                column: "OutStockId");

            migrationBuilder.CreateIndex(
                name: "IX_OutStockOrders_EmployeeId",
                table: "OutStockOrders",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_OutStockOrders_StockId",
                table: "OutStockOrders",
                column: "StockId");

            migrationBuilder.CreateIndex(
                name: "IX_PlanFees_PlanId",
                table: "PlanFees",
                column: "PlanId");

            migrationBuilder.CreateIndex(
                name: "IX_Plans_PlanName",
                table: "Plans",
                column: "PlanName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RetailShops_RegionId",
                table: "RetailShops",
                column: "RegionId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceBillDetails_BillId",
                table: "ServiceBillDetails",
                column: "BillId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceBills_CreateDate",
                table: "ServiceBills",
                column: "CreateDate");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceBills_ServiceOrderId",
                table: "ServiceBills",
                column: "ServiceOrderId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ServiceOrders_AccountId",
                table: "ServiceOrders",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceOrders_DateCreate",
                table: "ServiceOrders",
                column: "DateCreate");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceOrders_EmpIDCreater",
                table: "ServiceOrders",
                column: "EmpIDCreater");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceOrders_EmpIDSurveyor",
                table: "ServiceOrders",
                column: "EmpIDSurveyor");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceOrders_PlanFeeId",
                table: "ServiceOrders",
                column: "PlanFeeId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceOrders_RequestId",
                table: "ServiceOrders",
                column: "RequestId");

            migrationBuilder.CreateIndex(
                name: "IX_Stocks_RegionId",
                table: "Stocks",
                column: "RegionId");

            migrationBuilder.CreateIndex(
                name: "IX_SupportRequests_CustomerId",
                table: "SupportRequests",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_SupportRequests_EmpIdResolver",
                table: "SupportRequests",
                column: "EmpIdResolver");

            migrationBuilder.CreateIndex(
                name: "IX_SupportRequests_SupportRequestId",
                table: "SupportRequests",
                column: "SupportRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_Vendors_RegionId",
                table: "Vendors",
                column: "RegionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConnectionDiary");

            migrationBuilder.DropTable(
                name: "FeedBacks");

            migrationBuilder.DropTable(
                name: "InStockOrderDetails");

            migrationBuilder.DropTable(
                name: "InStockRequestDetails");

            migrationBuilder.DropTable(
                name: "NewsTB");

            migrationBuilder.DropTable(
                name: "OutStockOrderDetails");

            migrationBuilder.DropTable(
                name: "ServiceBillDetails");

            migrationBuilder.DropTable(
                name: "SupportRequests");

            migrationBuilder.DropTable(
                name: "Connections");

            migrationBuilder.DropTable(
                name: "InStockOrders");

            migrationBuilder.DropTable(
                name: "OutStockOrders");

            migrationBuilder.DropTable(
                name: "ServiceBills");

            migrationBuilder.DropTable(
                name: "Equipments");

            migrationBuilder.DropTable(
                name: "InStockRequests");

            migrationBuilder.DropTable(
                name: "ServiceOrders");

            migrationBuilder.DropTable(
                name: "Discounts");

            migrationBuilder.DropTable(
                name: "EquipmentTypes");

            migrationBuilder.DropTable(
                name: "Stocks");

            migrationBuilder.DropTable(
                name: "Vendors");

            migrationBuilder.DropTable(
                name: "Accounts");

            migrationBuilder.DropTable(
                name: "CustomerRequests");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "PlanFees");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "EmployeeRoles");

            migrationBuilder.DropTable(
                name: "RetailShops");

            migrationBuilder.DropTable(
                name: "Plans");

            migrationBuilder.DropTable(
                name: "Regions");
        }
    }
}
