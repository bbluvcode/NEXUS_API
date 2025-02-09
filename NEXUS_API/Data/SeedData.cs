using Microsoft.EntityFrameworkCore;
using NEXUS_API.Models;

namespace NEXUS_API.Data
{
    public class SeedData
    {
        public static void SeedingData(DatabaseContext _dbContext)
        {
            _dbContext.Database.Migrate();
            //Seed Keywords
            if (!_dbContext.Keywords.Any())
            {
                var keywords = new List<Keyword>()
                {
                    new Keyword
                    {
                        Words = "fuck",
                        Status = true,
                    },
                    new Keyword
                    {
                        Words = "fck",
                        Status = true,
                    },
                    new Keyword
                    {
                        Words = "dog",
                        Status = true,
                    },
                };
                _dbContext.Keywords.AddRange(keywords);
                _dbContext.SaveChanges();
            }
            // Seed Plan
            if (!_dbContext.Plans.Any())
            {
                var plans = new List<Plan>
                {
                    new Plan { PlanName = "Dial – Up Connection", SecurityDeposit = 325, Description = "Dial-up Internet service.", IsUsing = true},
                    new Plan { PlanName = "Broad Band Connection", SecurityDeposit = 500, Description = "High-speed broadband.", IsUsing = true},
                    new Plan { PlanName = "LandLine Connection", SecurityDeposit = 250, Description = "Landline telephone service.", IsUsing = true}
                };
                _dbContext.Plans.AddRange(plans);
                _dbContext.SaveChanges();
            }

            if (!_dbContext.PlanFees.Any())
            {
                var planFees = new List<PlanFee>
                {
                    // Dial-Up Connection
                    new PlanFee { PlanFeeName = "10 Hrs.", Description = "10 Hours - Valid for 1 Month", Rental = 50, PlanId = 1, IsUsing = true },
                    new PlanFee { PlanFeeName = "30 Hrs.", Description = "30 Hours - Valid for 3 Months", Rental = 130, PlanId = 1, IsUsing = true },
                    new PlanFee { PlanFeeName = "60 Hrs.", Description = "60 Hours - Valid for 6 Months", Rental = 260, PlanId = 1, IsUsing = true },
                    new PlanFee { PlanFeeName = "Unlimited 28Kbps Monthly", Description = "Unlimited Monthly Plan", Rental = 75, PlanId = 1, IsUsing = true },
                    new PlanFee { PlanFeeName = "Unlimited 28Kbps Quarterly", Description = "Unlimited Quarterly Plan", Rental = 150, PlanId = 1, IsUsing = true },
                    new PlanFee { PlanFeeName = "Unlimited 56Kbps Monthly", Description = "Unlimited Monthly Plan", Rental = 100, PlanId = 1, IsUsing = true },
                    new PlanFee { PlanFeeName = "Unlimited 56Kbps Quarterly", Description = "Unlimited Quarterly Plan", Rental = 180, PlanId = 1, IsUsing = true },

                    // Broadband Connection
                    new PlanFee { PlanFeeName = "30 Hrs.", Description = "30 Hours - Valid for 1 Month", Rental = 175, PlanId = 2, IsUsing = true },
                    new PlanFee { PlanFeeName = "60 Hrs.", Description = "60 Hours - Valid for 6 Months", Rental = 315, PlanId = 2, IsUsing = true },
                    new PlanFee { PlanFeeName = "Unlimited 64Kbps Monthly", Description = "Unlimited Monthly Plan", Rental = 225, PlanId = 2, IsUsing = true },
                    new PlanFee { PlanFeeName = "Unlimited 64Kbps Quarterly", Description = "Unlimited Quarterly Plan", Rental = 400, PlanId = 2, IsUsing = true },
                    new PlanFee { PlanFeeName = "Unlimited 128Kbps Monthly", Description = "Unlimited Monthly Plan", Rental = 350, PlanId = 2, IsUsing = true },
                    new PlanFee { PlanFeeName = "Unlimited 128Kbps Quarterly", Description = "Unlimited Quarterly Plan", Rental = 445, PlanId = 2, IsUsing = true },

                    // LandLine Connection
                    new PlanFee { PlanFeeName = "Local Unlimited", Description = "Unlimited - Valid for an year", Rental = 75, CallCharge = 0.55m, PlanId = 3, IsUsing = true },
                    new PlanFee { PlanFeeName = "Local Monthly", Description = "Monthly Plan", Rental = 35, CallCharge = 0.75m, PlanId = 3, IsUsing = true },
                    new PlanFee { PlanFeeName = "STD Monthly", Description = "Monthly Plan", Rental = 125, LocalCallCharge = 0.70m, DTDCallCharge = 2.25m, MessageMobileCharge = 1.00m, PlanId = 3, IsUsing = true },
                    new PlanFee { PlanFeeName = "STD Half-Yearly", Description = "Half-Yearly Plan", Rental = 420, LocalCallCharge = 0.60m, DTDCallCharge = 2.00m, MessageMobileCharge = 1.15m, PlanId = 3, IsUsing = true },
                    new PlanFee { PlanFeeName = "STD Yearly", Description = "Yearly Plan", Rental = 600, LocalCallCharge = 0.60m, DTDCallCharge = 1.75m, MessageMobileCharge = 1.25m, PlanId = 3, IsUsing = true }
                };

                _dbContext.PlanFees.AddRange(planFees);
                _dbContext.SaveChanges();
            }

            // Seed Regions
            if (!_dbContext.Regions.Any())
            {
                var regions = new List<Region>
            {
                new Region
                {
                    RegionCode = "HCMC-001",
                    RegionName = "Ho Chi Minh City",
                    Latitude = 10.8231,
                    Longitude = 106.6297
                },
                new Region
                {
                    RegionCode = "HN-002",
                    RegionName = "Ha Noi",
                    Latitude = 21.0285,
                    Longitude = 105.8542
                },
                new Region
                {
                    RegionCode = "DN-003",
                    RegionName = "Da Nang",
                    Latitude = 16.0471,
                    Longitude = 108.2068
                },
                new Region
                {
                    RegionCode = "ASIA-004",
                    RegionName = "Asia Headquarters",
                    Latitude = 0.0,  // Use an appropriate location or a central location
                    Longitude = 0.0
                },
                new Region
                {
                    RegionCode = "SG-005",
                    RegionName = "Singapore",
                    Latitude = 1.3521,
                    Longitude = 103.8198
                },
                new Region
                {
                    RegionCode = "KL-006",
                    RegionName = "Kuala Lumpur",
                    Latitude = 3.1390,
                    Longitude = 101.6869
                },
                new Region
                {
                    RegionCode = "BKK-007",
                    RegionName = "Bangkok",
                    Latitude = 13.7563,
                    Longitude = 100.5018
                }
            };
                _dbContext.Regions.AddRange(regions);
                _dbContext.SaveChanges();
            }
            // Seed Vendors
            if (!_dbContext.Vendors.Any())
            {
                var vendors = new List<Vendor>
                {
                    new Vendor
                    {
                        VendorName = "Tech Supply Co.",
                        Address = "123 Nguyen Hue, Ho Chi Minh City",
                        Email = "contact@techsupply.com",
                        Phone = "0901234567",
                        Fax = "0281234567",
                        Description = "Leading supplier of IT equipment.",
                        Status = true,
                        RegionId = _dbContext.Regions.FirstOrDefault(r => r.RegionCode == "HCMC-001")?.RegionId
                    },
                    new Vendor
                    {
                        VendorName = "Hanoi Elec",
                        Address = "456 Le Duan, Ha Noi",
                        Email = "info@hanoielec.vn",
                        Phone = "0987654321",
                        Fax = "0241234567",
                        Description = "Trusted provider of electronic components.",
                        Status = true,
                        RegionId = _dbContext.Regions.FirstOrDefault(r => r.RegionCode == "HN-002")?.RegionId
                    },
                    new Vendor
                    {
                        VendorName = "AsiaTech",
                        Address = "789 Tran Phu, Da Nang",
                        Email = "support@asiatech.com",
                        Phone = "0912345678",
                        Fax = "0511234567",
                        Description = "Supplier of networking and security equipment.",
                        Status = true,
                        RegionId = _dbContext.Regions.FirstOrDefault(r => r.RegionCode == "DN-003")?.RegionId
                    },
                    new Vendor
                    {
                        VendorName = "SG Traders",
                        Address = "12 Orchard Road, Singapore",
                        Email = "sales@sgtraders.com",
                        Phone = "6512345678",
                        Fax = "6512345679",
                        Description = "Leading B2B distributor in Southeast Asia.",
                        Status = true,
                        RegionId = _dbContext.Regions.FirstOrDefault(r => r.RegionCode == "SG-005")?.RegionId
                    },
                    new Vendor
                    {
                        VendorName = "BKK Imports",
                        Address = "88 Sukhumvit, Bangkok",
                        Email = "contact@bkkimports.com",
                        Phone = "6612345678",
                        Fax = "6612345679",
                        Description = "Specializing in hardware and industrial equipment.",
                        Status = true,
                        RegionId = _dbContext.Regions.FirstOrDefault(r => r.RegionCode == "BKK-007")?.RegionId
                    }
                };

                _dbContext.Vendors.AddRange(vendors);
                _dbContext.SaveChanges();
            }
            // Seed EmployeeRoles
            if (!_dbContext.EmployeeRoles.Any())
            {
                var roles = new List<EmployeeRole>
                {
                    new EmployeeRole { RoleName = "Admin" },
                    new EmployeeRole { RoleName = "ManagerShop" },
                    new EmployeeRole { RoleName = "Accountant" },
                    new EmployeeRole { RoleName = "ITSupport" },
                    new EmployeeRole { RoleName = "Technical" },
                    new EmployeeRole { RoleName = "Surveyor" },
                    new EmployeeRole { RoleName = "CustomerSupport" },
                    new EmployeeRole { RoleName = "SaleShop" },
                };
                _dbContext.EmployeeRoles.AddRange(roles);
                _dbContext.SaveChanges();
            }
            if (!_dbContext.RetailShops.Any())
            {
                var retailShops = new List<RetailShop>
                {
                    new RetailShop
                    {
                        RetailShopName = "Main Office",
                        Address = "123 Main St, District 1, HCMC",
                        Email = "mainoffice@nexus.com",
                        Phone = "0123456789",
                        IsMainOffice = true,
                        Fax = "012345678",
                        RegionId = 1, // Replace with a valid RegionId in your database
                        Image = "/Images/imageRetail/main_office.jpg",
                        Status = true
                    },
                    new RetailShop
                    {
                        RetailShopName = "Branch A",
                        Address = "456 Branch St, District 3, HCMC",
                        Email = "brancha@nexus.com",
                        Phone = "0987654321",
                        IsMainOffice = false,
                        Fax = "987654321",
                        RegionId = 2, // Replace with a valid RegionId in your database
                        Image = "/Images/imageRetail/branch_a.jpg",
                        Status = true
                    },
                    new RetailShop
                    {
                        RetailShopName = "Branch B",
                        Address = "789 Second Ave, District 7, HCMC",
                        Email = "branchb@nexus.com",
                        Phone = "0912345678",
                        IsMainOffice = false,
                        Fax = "091234567",
                        RegionId = 3, // Replace with a valid RegionId in your database
                        Image = "/Images/imageRetail/branch_b.jpg",
                        Status = true
                    },
                    new RetailShop
                    {
                        RetailShopName = "Asia HQ",
                        Address = "789 Asia Center, Singapore",
                        Email = "asiahq@nexus.com",
                        Phone = "0923456789",
                        IsMainOffice = false,
                        Fax = "092345678",
                        RegionId = 4, // Asia Headquarters
                        Image = "/Images/imageRetail/asia_hq.jpg",
                        Status = true
                    },
                    new RetailShop
                    {
                        RetailShopName = "Singapore Branch",
                        Address = "123 Orchard Rd, Singapore",
                        Email = "sgbranch@nexus.com",
                        Phone = "0911122233",
                        IsMainOffice = false,
                        Fax = "0911122233",
                        RegionId = 5, // Singapore
                        Image = "/Images/imageRetail/sg_branch.jpg",
                        Status = true
                    },
                    new RetailShop
                    {
                        RetailShopName = "Kuala Lumpur Branch",
                        Address = "45 Jalan Ampang, Kuala Lumpur",
                        Email = "klbranch@nexus.com",
                        Phone = "0909988776",
                        IsMainOffice = false,
                        Fax = "0909988776",
                        RegionId = 6, // Kuala Lumpur
                        Image = "/Images/imageRetail/kl_branch.jpg",
                        Status = true
                    },
                    new RetailShop
                    {
                        RetailShopName = "Bangkok Branch",
                        Address = "99 Sukhumvit Rd, Bangkok",
                        Email = "bkkbranch@nexus.com",
                        Phone = "0988765432",
                        IsMainOffice = false,
                        Fax = "0988765432",
                        RegionId = 7, // Bangkok
                        Image = "/Images/imageRetail/bkk_branch.jpg",
                        Status = true
                    }
                };

                _dbContext.RetailShops.AddRange(retailShops);
                _dbContext.SaveChanges();
            }



            // Seed Employees
            if (!_dbContext.Employees.Any())
            {
                var employees = new List<Employee>
                {
                    new Employee
                    {
                        EmployeeCode = "E001",
                        FullName = "Nguyen Van A",
                        Gender = "Male",
                        DateOfBirth = new DateTime(1985, 3, 15),
                        Address = "12 Nguyen Trai, District 1, HCMC",
                        Email = "nva@nexus.com",
                        Phone = "0901234567",
                        IdentificationNo = "123456789",
                        Password = "password123",
                        Status = true,
                        EmployeeRoleId = 1, 
                        RetailShopId = 1 
                    },
                    new Employee
                    {
                        EmployeeCode = "E002",
                        FullName = "Tran Thi B",
                        Gender = "Female",
                        DateOfBirth = new DateTime(1990, 7, 10),
                        Address = "45 Le Loi, District 3, HCMC",
                        Email = "ttb@nexus.com",
                        Phone = "0909876543",
                        IdentificationNo = "987654321",
                        Password = "password456",
                        Status = true,
                        EmployeeRoleId = 2, 
                        RetailShopId = 2 
                    },
                    new Employee
                    {
                        EmployeeCode = "E003",
                        FullName = "Lee Wei",
                        Gender = "Male",
                        DateOfBirth = new DateTime(1992, 4, 5),
                        Address = "88 Chinatown, Singapore",
                        Email = "leewei@nexus.com",
                        Phone = "0811223344",
                        IdentificationNo = "2233445566",
                        Password = "password789",
                        Status = true,
                        EmployeeRoleId = 3, 
                        RetailShopId = 3
                    },
                    new Employee
                    {
                        EmployeeCode = "E004",
                        FullName = "Pham Van C",
                        Gender = "Male",
                        DateOfBirth = new DateTime(1988, 5, 20),
                        Address = "100 Vo Van Tan, District 1, HCMC",
                        Email = "pvc@nexus.com",
                        Phone = "0912345678",
                        IdentificationNo = "234567890",
                        Password = "password234",
                        Status = true,
                        EmployeeRoleId = 2,
                        RetailShopId = 1 
                    },
                    new Employee
                    {
                        EmployeeCode = "E005",
                        FullName = "Hoang Minh D",
                        Gender = "Male",
                        DateOfBirth = new DateTime(1993, 9, 25),
                        Address = "200 Bach Dang, District 2, HCMC",
                        Email = "hmd@nexus.com",
                        Phone = "0923456789",
                        IdentificationNo = "345678901",
                        Password = "password345",
                        Status = true,
                        EmployeeRoleId = 5, 
                        RetailShopId = 2 
                    },
                    new Employee
                    {
                        EmployeeCode = "E006",
                        FullName = "Le Thi E",
                        Gender = "Female",
                        DateOfBirth = new DateTime(1995, 6, 18),
                        Address = "300 Tran Hung Dao, District 5, HCMC",
                        Email = "lte@nexus.com",
                        Phone = "0934567890",
                        IdentificationNo = "456789012",
                        Password = "password456",
                        Status = true,
                        EmployeeRoleId = 6, 
                        RetailShopId = 3 
                    },
                    new Employee
                    {
                        EmployeeCode = "E007",
                        FullName = "Nguyen Hoang F",
                        Gender = "Male",
                        DateOfBirth = new DateTime(1991, 8, 12),
                        Address = "400 Ly Tu Trong, District 10, HCMC",
                        Email = "nhf@nexus.com",
                        Phone = "0945678901",
                        IdentificationNo = "567890123",
                        Password = "password567",
                        Status = true,
                        EmployeeRoleId = 2, 
                        RetailShopId = 3 
                    },
                    new Employee
                    {
                        EmployeeCode = "E008",
                        FullName = "Bui Thanh G",
                        Gender = "Male",
                        DateOfBirth = new DateTime(1989, 11, 30),
                        Address = "500 Hai Ba Trung, District 6, HCMC",
                        Email = "btg@nexus.com",
                        Phone = "0956789012",
                        IdentificationNo = "678901234",
                        Password = "password678",
                        Status = true,
                        EmployeeRoleId = 5, 
                        RetailShopId = 1 
                    },
                    new Employee
                    {
                        EmployeeCode = "E009",
                        FullName = "Doan Thanh H",
                        Gender = "Female",
                        DateOfBirth = new DateTime(1994, 2, 22),
                        Address = "600 Phan Xich Long, Phu Nhuan, HCMC",
                        Email = "dth@nexus.com",
                        Phone = "0967890123",
                        IdentificationNo = "789012345",
                        Password = "password789",
                        Status = true,
                        EmployeeRoleId = 6,
                        RetailShopId = 2
                    },
                    new Employee { EmployeeCode = "E010", FullName = "Pham Van C", Gender = "Male", DateOfBirth = new DateTime(1988, 5, 20), Address = "100 Vo Van Tan, District 1, HCMC", Email = "pvc@nexus.com", Phone = "0912345678", IdentificationNo = "234567890", Password = "password234", Status = true, EmployeeRoleId = 5, RetailShopId = 1 }, // ManagerShop
                    new Employee { EmployeeCode = "E011", FullName = "Hoang Minh D", Gender = "Male", DateOfBirth = new DateTime(1993, 9, 25), Address = "200 Bach Dang, District 2, HCMC", Email = "hmd@nexus.com", Phone = "0923456789", IdentificationNo = "345678901", Password = "password345", Status = true, EmployeeRoleId = 5, RetailShopId = 1 }, // Technical
                    new Employee { EmployeeCode = "E012", FullName = "Le Thi E", Gender = "Female", DateOfBirth = new DateTime(1995, 6, 18), Address = "300 Tran Hung Dao, District 5, HCMC", Email = "lte@nexus.com", Phone = "0934567890", IdentificationNo = "456789012", Password = "password456", Status = true, EmployeeRoleId = 5, RetailShopId = 1 }, // Technical
                    new Employee { EmployeeCode = "E013", FullName = "Nguyen Hoang F", Gender = "Male", DateOfBirth = new DateTime(1991, 8, 12), Address = "400 Ly Tu Trong, District 10, HCMC", Email = "nhf@nexus.com", Phone = "0945678901", IdentificationNo = "567890123", Password = "password567", Status = true, EmployeeRoleId = 6, RetailShopId = 1 }, // Surveyor

                    // RetailShopId = 2
                    new Employee { EmployeeCode = "E014", FullName = "Tran Quoc G", Gender = "Male", DateOfBirth = new DateTime(1987, 7, 14), Address = "101 Nguyen Hue, District 1, HCMC", Email = "tqg@nexus.com", Phone = "0971234567", IdentificationNo = "678901234", Password = "password678", Status = true, EmployeeRoleId = 5, RetailShopId = 2 }, // ManagerShop
                    new Employee { EmployeeCode = "E015", FullName = "Bui Thanh H", Gender = "Male", DateOfBirth = new DateTime(1992, 3, 22), Address = "102 Pasteur, District 3, HCMC", Email = "bth@nexus.com", Phone = "0982345678", IdentificationNo = "789012345", Password = "password789", Status = true, EmployeeRoleId = 5, RetailShopId = 2 }, // Technical
                    new Employee { EmployeeCode = "E016", FullName = "Doan Thanh I", Gender = "Female", DateOfBirth = new DateTime(1994, 6, 30), Address = "103 Hai Ba Trung, District 1, HCMC", Email = "dti@nexus.com", Phone = "0993456789", IdentificationNo = "890123456", Password = "password890", Status = true, EmployeeRoleId = 5, RetailShopId = 2 }, // Technical
                    new Employee { EmployeeCode = "E017", FullName = "Vu Thi K", Gender = "Female", DateOfBirth = new DateTime(1995, 9, 8), Address = "104 Dong Khoi, District 1, HCMC", Email = "vtk@nexus.com", Phone = "0914567890", IdentificationNo = "901234567", Password = "password901", Status = true, EmployeeRoleId = 6, RetailShopId = 2 }, // Surveyor

                    // RetailShopId = 3
                    new Employee { EmployeeCode = "E018", FullName = "Nguyen Xuan L", Gender = "Male", DateOfBirth = new DateTime(1990, 12, 15), Address = "105 Le Thanh Ton, District 1, HCMC", Email = "nxl@nexus.com", Phone = "0925678901", IdentificationNo = "012345678", Password = "password012", Status = true, EmployeeRoleId = 2, RetailShopId = 3 }, // ManagerShop
                    new Employee { EmployeeCode = "E019", FullName = "Hoang Duc M", Gender = "Male", DateOfBirth = new DateTime(1986, 2, 10), Address = "106 Nguyen Dinh Chieu, District 3, HCMC", Email = "hdm@nexus.com", Phone = "0936789012", IdentificationNo = "123456789", Password = "password123", Status = true, EmployeeRoleId = 5, RetailShopId = 3 }, // Technical
                    new Employee { EmployeeCode = "E020", FullName = "Le Quang N", Gender = "Male", DateOfBirth = new DateTime(1993, 5, 25), Address = "107 Vo Thi Sau, District 1, HCMC", Email = "lqn@nexus.com", Phone = "0947890123", IdentificationNo = "234567890", Password = "password234", Status = true, EmployeeRoleId = 5, RetailShopId = 3 }, // Technical
                    new Employee { EmployeeCode = "E021", FullName = "Pham Minh O", Gender = "Female", DateOfBirth = new DateTime(1991, 10, 8), Address = "108 Pham Ngoc Thach, District 3, HCMC", Email = "pmo@nexus.com", Phone = "0958901234", IdentificationNo = "345678901", Password = "password345", Status = true, EmployeeRoleId = 6, RetailShopId = 3 } // Surveyor
                };

                _dbContext.Employees.AddRange(employees);
                _dbContext.SaveChanges();
            }

            // Seed dữ liệu Customers
            if (!_dbContext.Customers.Any())
            {
                _dbContext.Customers.AddRange(
                    new Customer
                    {
                        FullName = "test",
                        Gender = "Male",
                        DateOfBirth = new DateTime(1990, 5, 20),
                        Address = "test",
                        Email = "test@gmail.com",
                        PhoneNumber = "09012345675",
                        IdentificationNo = "123456789",
                        Image = "test",
                        Password = "password123",
                        RefreshToken = null,
                        RefreshTokenExpried = null,
                        FailedLoginAttempts = 0,
                        ExpiredBan = null,
                        Code = null,
                        ExpiredCode = null,
                        SendCodeAttempts = 0,
                        LastSendCodeDate = null
                    },
                    new Customer
                    {
                        FullName = "Nguyen Van A",
                        Gender = "Male",
                        DateOfBirth = new DateTime(1990, 5, 20),
                        Address = "123 Nguyen Trai, Quan 1, TP.HCM",
                        Email = "nguyenvana@gmail.com",
                        PhoneNumber = "0901234567",
                        IdentificationNo = "123456789",
                        Image = "string",
                        Password = "password123",
                        RefreshToken = null,
                        RefreshTokenExpried = null,
                        FailedLoginAttempts = 0,
                        ExpiredBan = null,
                        Code = null,
                        ExpiredCode = null,
                        SendCodeAttempts = 0,
                        LastSendCodeDate = null
                    },
                    new Customer
                    {
                        FullName = "Tran Thi B",
                        Gender = "Female",
                        DateOfBirth = new DateTime(1995, 8, 15),
                        Address = "456 Le Loi, Quan 3, TP.HCM",
                        Email = "tranthib@gmail.com",
                        PhoneNumber = "0912345678",
                        IdentificationNo = "987654321",
                        Image = "string",
                        Password = "password456",
                        RefreshToken = null,
                        RefreshTokenExpried = null,
                        FailedLoginAttempts = 0,
                        ExpiredBan = null,
                        Code = null,
                        ExpiredCode = null,
                        SendCodeAttempts = 0,
                        LastSendCodeDate = null
                    },
                    new Customer
                    {
                        FullName = "Le Van C",
                        Gender = "Male",
                        DateOfBirth = new DateTime(1988, 12, 10),
                        Address = "789 Cach Mang Thang 8, Tan Binh, TP.HCM",
                        Email = "levanc@gmail.com",
                        PhoneNumber = "0923456789",
                        IdentificationNo = "1122334455",
                        Image = "string",
                        Password = "password789",
                        RefreshToken = null,
                        RefreshTokenExpried = null,
                        FailedLoginAttempts = 0,
                        ExpiredBan = null,
                        Code = null,
                        ExpiredCode = null,
                        SendCodeAttempts = 0,
                        LastSendCodeDate = null
                    }
                );

                _dbContext.SaveChanges();
            }

            // Seed dữ liệu Customers Request
            if (!_dbContext.CustomerRequests.Any())
            {
                var customerRequests = new List<CustomerRequest>
                {
                    new CustomerRequest
                    {
                        RequestTitle = "test",
                        ServiceRequest = "test",
                        EquipmentRequest = "test",
                        DateCreate = new DateTime(2024,1,10),                        
                        IsResponse = false,
                        Deposit = 10,
                        DepositStatus = "Paid",
                        RegionId = 1,
                        CustomerId = 1
                    },
                    new CustomerRequest
                    {
                        RequestTitle = "Broadband Installation",
                        ServiceRequest = "Request to install a broadband internet connection.",
                        EquipmentRequest = "Broadband Router Model BR123",
                        DateCreate = new DateTime(2024,1,10),
                        IsResponse = false,
                        Deposit = 10,
                        DepositStatus = "Paid",
                        RegionId = 1,
                        CustomerId = 1
                    },
                    new CustomerRequest
                    {
                        RequestTitle = "Land-line Issue",
                        ServiceRequest = "The land-line phone has no dial tone.",
                        EquipmentRequest = "Replacement Handset Model LH456",
                        DateCreate = new DateTime(2024,1,10),
                        IsResponse = false,
                        Deposit = 10,
                        DepositStatus = "pending",
                        RegionId = 1,
                        CustomerId = 1
                    },
                    new CustomerRequest
                    {
                        RequestTitle = "Dial-up Assistance",
                        ServiceRequest = "Unable to connect to the internet using dial-up.",
                        EquipmentRequest = "Dial-up Modem Model DM789",
                        DateCreate = new DateTime(2024,1,10),
                        IsResponse = false,
                        Deposit = 10,
                        DepositStatus = "Paid",
                        RegionId = 1,
                        CustomerId = 1
                    },
                    new CustomerRequest
                    {
                        RequestTitle = "Broadband Speed Issue",
                        ServiceRequest = "Broadband connection is slower than expected.",
                        EquipmentRequest = null,
                        DateCreate = new DateTime(2024,1,10),
                        IsResponse = false,
                        Deposit = 10,
                        DepositStatus = "pending",
                        RegionId = 1,
                        CustomerId = 2
                    },
                    new CustomerRequest
                    {
                        RequestTitle = "Land-line Installation",
                        ServiceRequest = "Request to install a new land-line phone.",
                        EquipmentRequest = "Land-line Phone Model LL321",
                        DateCreate = new DateTime(2024,1,10),
                        IsResponse = false,
                        Deposit = 10,
                        DepositStatus = "Paid",
                        RegionId = 1,
                        CustomerId = 2,
                    },
                    new CustomerRequest
                    {
                        RequestTitle = "Dial-up Account Setup",
                        ServiceRequest = "Need help setting up a new dial-up account.",
                        EquipmentRequest = null,
                        DateCreate = new DateTime(2024,1,10),
                        IsResponse = false,
                        Deposit = 30,
                        DepositStatus = "Paid",
                        RegionId = 1,
                        CustomerId = 2
                    },
                    new CustomerRequest
                    {
                        RequestTitle = "Broadband Router Replacement",
                        ServiceRequest = "The broadband router is not working and needs replacement.",
                        EquipmentRequest = "New Router Model BR999",
                        DateCreate = new DateTime(2024,1,10),
                        IsResponse = false,
                        Deposit = 10,
                        DepositStatus = "Paid",
                        RegionId = 1,
                        CustomerId = 3
                    },
                    new CustomerRequest
                    {
                        RequestTitle = "Land-line Noise Issue",
                        ServiceRequest = "There is excessive noise on the land-line during calls.",
                        EquipmentRequest = null,
                        DateCreate = new DateTime(2024,1,10),
                        IsResponse = false,
                        Deposit = 60,
                        DepositStatus = "Paid",
                        RegionId = 1,
                        CustomerId = 3
                    },
                    new CustomerRequest
                    {
                        RequestTitle = "Dial-up Connection Issue",
                        ServiceRequest = "Frequent disconnections while using dial-up internet.",
                        EquipmentRequest = "Replacement Modem Model DM111",
                        DateCreate = new DateTime(2024,1,10),
                        IsResponse = false,
                        Deposit = 25,
                        DepositStatus = "Paid",
                        RegionId = 1,
                        CustomerId = 3
                    },
                    new CustomerRequest
                    {
                        RequestTitle = "test",
                        ServiceRequest = "test",
                        EquipmentRequest = "test",
                        DateCreate = new DateTime(2024,1,10),
                        IsResponse = false,
                        Deposit = 20,
                        DepositStatus = "pending",
                        RegionId = 1,
                        CustomerId = 3
                    }
                };

                _dbContext.CustomerRequests.AddRange(customerRequests);
                _dbContext.SaveChanges();
            }

            // Seed SupportRequests
            if (!_dbContext.SupportRequests.Any())
            {
                var supportRequests = new List<SupportRequest>
            {
                new SupportRequest
                {
                    DateRequest = new DateTime(2024, 1, 5),
                    Title = "test",
                    DetailContent = "test",
                    DateResolved = null,
                    IsResolved = false,
                    Email = "bbluvcode@gmail.com", // Replace with valid Email
                    EmpIdResolver = null // No resolver yet
                },
                new SupportRequest
                {
                    DateRequest = new DateTime(2024, 1, 5),
                    Title = "Request for Broadband Repair",
                    DetailContent = "The broadband connection is not working since yesterday evening.",
                    DateResolved = null,
                    IsResolved = false,
                    Email = "bbluvcode@gmail.com", // Replace with valid Email
                    EmpIdResolver = null // No resolver yet
                },
                new SupportRequest
                {
                    DateRequest = new DateTime(2024, 1, 10),
                    Title = "Request for Land-line Repair",
                    DetailContent = "Land-line phone is not working properly; there is a lot of static noise.",
                    DateResolved = new DateTime(2024, 1, 12),
                    IsResolved = true,
                    Email = "bbluvcode@gmail.com", // Replace with valid Email
                    EmpIdResolver = 1 // EmployeeId of resolver
                },
                new SupportRequest
                {
                    DateRequest = new DateTime(2024, 1, 15),
                    Title = "Help with Dial-up Internet Configuration",
                    DetailContent = "Need assistance in configuring the dial-up connection for better speed.",
                    DateResolved = null,
                    IsResolved = false,
                    Email = "bbluvcode@gmail.com", // Replace with valid Email
                    EmpIdResolver = null // No resolver yet
                },
            };
                _dbContext.SupportRequests.AddRange(supportRequests);
                _dbContext.SaveChanges();
            }
            
            //seed equipment type
            if (!_dbContext.EquipmentTypes.Any())
            {
                var equipmentTypes = new List<EquipmentType>
                    {
                        new EquipmentType { TypeName = "Dial-Up Modem v1", Provider = "U.S. Robotics" },
                        new EquipmentType { TypeName = "Dial-Up Modem v2", Provider = "Zoom" },
                        new EquipmentType { TypeName = "Dial-Up Modem v3", Provider = "Trendnet" },
                        new EquipmentType { TypeName = "ADSL Modem v1", Provider = "TP-Link A" },
                        new EquipmentType { TypeName = "ADSL Modem v2", Provider = "D-Link" },
                        new EquipmentType { TypeName = "ADSL Modem v3", Provider = "Zyxel" },
                        new EquipmentType { TypeName = "Cable Modem v1", Provider = "ARRIS" },
                        new EquipmentType { TypeName = "Cable Modem v2", Provider = "Netgear" },
                        new EquipmentType { TypeName = "Cable Modem v3", Provider = "Motorola" },
                        new EquipmentType { TypeName = "4G Modem v1", Provider = "Huawei" },
                        new EquipmentType { TypeName = "4G Modem v2", Provider = "Cisco" }, 
                        new EquipmentType { TypeName = "4G Modem v3", Provider = "TP-Link" }
                    };

                _dbContext.EquipmentTypes.AddRange(equipmentTypes);
                _dbContext.SaveChanges();
            }

            //seed stock
            if (!_dbContext.Stocks.Any())
            {
                var stocks = new List<Stock>
            {
                new Stock { StockName = "Stock A", Address = "123 Main St, City A", Email = "stocka@example.com", Phone = "1234567890", Fax = "1234567890", RegionId = 1 },
                new Stock { StockName = "Stock B", Address = "456 Side St, City B", Email = "stockb@example.com", Phone = "0987654321", Fax = "0987654321", RegionId = 2 },
                new Stock { StockName = "Stock C", Address = "789 Center St, City C", Email = "stockc@example.com", Phone = "1122334455", Fax = "1122334455", RegionId = 3 },
                new Stock { StockName = "Stock D", Address = "101 Highway St, City D", Email = "stockd@example.com", Phone = "5566778899", Fax = "5566778899", RegionId = 4 }
            };

                _dbContext.Stocks.AddRange(stocks);
                _dbContext.SaveChanges();
            }
            //seed equipment
            //if (!_dbContext.Equipments.Any())
            //{
            //    var equipments = new List<Equipment>
            //    {
            //        new Equipment {
            //            EquipmentName = "U.S. Robotics 56K USB Modem", Price = 49.99M, StockQuantity = 100, Description = "Modem Dial-Up tốc độ 56Kbps", Status = true, EquipmentTypeId = 37, VendorId = 1, StockId = 2,
            //            Image = "/Image/imageEquipment/equip1.jpg"
            //        },
            //        new Equipment { 
            //            EquipmentName = "Zoom 3095 V.92 USB Modem", Price = 39.99M, StockQuantity = 80, Description = "Modem Dial-Up hỗ trợ V.92", Status = true, EquipmentTypeId = 38, VendorId = 2, StockId = 2,
            //            Image = "/Image/imageEquipment/equip2.jpg"
            //        },
            //        new Equipment { 
            //            EquipmentName = "Trendnet TFM-561U", Price = 29.99M, StockQuantity = 120, Description = "Modem Dial-Up USB cho Windows/Mac", Status = true, EquipmentTypeId = 39, VendorId = 3, StockId = 2 ,
            //            Image = "/Image/imageEquipment/equip3.jpg"
            //        },
            //        new Equipment {
            //            EquipmentName = "TP-Link TD-W8961N", Price = 59.99M, StockQuantity = 50, Description = "Modem ADSL2+ tích hợp Wi-Fi", Status = true, EquipmentTypeId = 40, VendorId = 4, StockId = 2 ,
            //            Image = "/Image/imageEquipment/equip4.jpg"
            //        },
            //        new Equipment { EquipmentName = "D-Link DSL-2750U", Price = 54.99M, StockQuantity = 70, Description = "Modem ADSL2+ tốc độ cao", Status = true, EquipmentTypeId = 41, VendorId = 5, StockId = 2 ,
            //            Image = "/Image/imageEquipment/equip5.jpg"
            //        },
            //        new Equipment { EquipmentName = "Zyxel P-600 Series", Price = 45.99M, StockQuantity = 90, Description = "Modem ADSL tiêu chuẩn", Status = true, EquipmentTypeId = 42, VendorId = 1, StockId = 2 ,
            //            Image = "/Image/imageEquipment/equip6.jpg"
            //        },
            //        new Equipment { EquipmentName = "ARRIS SURFboard SB8200", Price = 129.99M, StockQuantity = 40, Description = "Modem Cable DOCSIS 3.1", Status = true, EquipmentTypeId = 43, VendorId = 1, StockId = 3 ,
            //            Image = "/Image/imageEquipment/equip7.jpg"
            //        },
            //        new Equipment { EquipmentName = "Netgear CM500", Price = 89.99M, StockQuantity = 60, Description = "Modem Cable DOCSIS 3.0 tốc độ cao", Status = true, EquipmentTypeId = 48, VendorId = 2, StockId = 3 ,
            //            Image = "/Image/imageEquipment/equip8.jpg"
            //        },
            //    };

            //    _dbContext.Equipments.AddRange(equipments);
            //    _dbContext.SaveChanges();
            //}

            //seed instockrequest
            if (!_dbContext.InStockRequests.Any())
            {
                var inStockRequests = new List<InStockRequest>
            {
                new InStockRequest
                {
                    EmployeeId = 1,
                    CreateDate = DateTime.UtcNow,
                    TotalNumber = 10
                },
                new InStockRequest
                {
                    EmployeeId = 2,
                    CreateDate = DateTime.UtcNow,
                    TotalNumber = 8
                }
            };

                _dbContext.InStockRequests.AddRange(inStockRequests);
                _dbContext.SaveChanges();
            }
            //seed instockrequestdetail
            //if (!_dbContext.InStockRequestDetails.Any())
            //{
            //    var inStockRequestDetails = new List<InStockRequestDetail>
            //{
            //    new InStockRequestDetail { InStockRequestId = 1, EquipmentId = 14, Quantity = 5 },
            //    new InStockRequestDetail { InStockRequestId = 1, EquipmentId = 15, Quantity = 3 },
            //    new InStockRequestDetail { InStockRequestId = 2, EquipmentId = 16, Quantity = 4 },
            //    new InStockRequestDetail { InStockRequestId = 2, EquipmentId = 17, Quantity = 2 }
            //};

            //    _dbContext.InStockRequestDetails.AddRange(inStockRequestDetails);
            //    _dbContext.SaveChanges();
            //}
            //seed Instockorder
            if (!_dbContext.InStockOrders.Any())
            {
                var inStockOrders = new List<InStockOrder>
            {
                new InStockOrder {
                    InStockRequestId = 1,
                    VendorId = 1,
                    EmployeeId = 1,
                    StockId = 1,
                    Payer = 1,
                    CreateDate = DateTime.UtcNow,
                    InstockDate = DateTime.UtcNow.AddDays(2),
                    PayDate = DateTime.UtcNow.AddDays(5),
                    Tax = 50.00m,
                    Total = 1050.00m,
                    CurrencyUnit = "USD",
                    isPay = true
                },
                new InStockOrder {
                    InStockRequestId = 2,
                    VendorId = 2,
                    EmployeeId = 2,
                    StockId = 2,
                    Payer = 2,
                    CreateDate = DateTime.UtcNow,
                    InstockDate = DateTime.UtcNow.AddDays(3),
                    PayDate = DateTime.UtcNow.AddDays(6),
                    Tax = 30.00m,
                    Total = 830.00m,
                    CurrencyUnit = "EUR",
                    isPay = false
                }
            };

                _dbContext.InStockOrders.AddRange(inStockOrders);
                _dbContext.SaveChanges();
            }
            //seed Instockorderdetail

            //if (!_dbContext.InStockOrderDetails.Any())
            //{
            //    var inStockOrderDetails = new List<InStockOrderDetail>
            //{
            //    new InStockOrderDetail {  InStockOrderId = 2, EquipmentId = 14, Quantity = 5, Price = 200.00m },
            //    new InStockOrderDetail {  InStockOrderId = 2, EquipmentId = 15, Quantity = 3, Price = 150.00m },
            //    new InStockOrderDetail {  InStockOrderId = 3, EquipmentId = 16, Quantity = 4, Price = 180.00m },
            //    new InStockOrderDetail {  InStockOrderId = 3, EquipmentId = 17, Quantity = 2, Price = 110.00m }
            //};

            //    _dbContext.InStockOrderDetails.AddRange(inStockOrderDetails);
            //    _dbContext.SaveChanges();
            //}
            //seed Outstockorder
            if (!_dbContext.OutStockOrders.Any())
            {
                var outStockOrders = new List<OutStockOrder>
            {
                new OutStockOrder
                {
                    StockId = 1,
                    EmployeeId = 1,
                    CreateDate = DateTime.UtcNow,
                    PayDate = DateTime.UtcNow.AddDays(5),
                    Tax = 50.00m,
                    Total = 1050.00m,
                    isPay = true
                },
                new OutStockOrder
                {
                    StockId = 2,
                    EmployeeId = 2,
                    CreateDate = DateTime.UtcNow,
                    PayDate = DateTime.UtcNow.AddDays(3),
                    Tax = 30.00m,
                    Total = 830.00m,
                    isPay = false
                }
            };

                _dbContext.OutStockOrders.AddRange(outStockOrders);
                _dbContext.SaveChanges();
            }
            //seed outstockorderdetail
            if (!_dbContext.OutStockOrderDetails.Any())
            {
                var outStockOrderDetails = new List<OutStockOrderDetail>
            {
                new OutStockOrderDetail { OutStockId = 1, EquipmentId = 14, Quantity = 5, Price = 200.00m },
                new OutStockOrderDetail { OutStockId = 1, EquipmentId = 17, Quantity = 3, Price = 150.00m },
                new OutStockOrderDetail { OutStockId = 2, EquipmentId = 18, Quantity = 4, Price = 180.00m },
                new OutStockOrderDetail { OutStockId = 2, EquipmentId = 21, Quantity = 2, Price = 110.00m }
            };

                _dbContext.OutStockOrderDetails.AddRange(outStockOrderDetails);
                _dbContext.SaveChanges();
            }
        }
    }
}
