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
                        EmployeeRoleId = 1, // Replace with valid RoleId
                        RetailShopId = 1 // Replace with valid RetailShopId
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
                        EmployeeRoleId = 2, // Replace with valid RoleId
                        RetailShopId = 2 // Replace with valid RetailShopId
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
                        EmployeeRoleId = 3, // Accountant
                        RetailShopId = 3 // Asia HQ
                    }
                    // Add more employees as needed
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
                        RegionId = 1,
                        CustomerId = 1
                    },
                    new CustomerRequest
                    {
                        RequestTitle = "Dial-up Assistance",
                        ServiceRequest = "Unable to connect to the internet using dial-up.",
                        EquipmentRequest = "Dial-up Modem Model DM789",
                        DateCreate = new DateTime(2024,1,10),
                        IsResponse = true,
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
                        RegionId = 1,
                        CustomerId = 2
                    },
                    new CustomerRequest
                    {
                        RequestTitle = "Land-line Installation",
                        ServiceRequest = "Request to install a new land-line phone.",
                        EquipmentRequest = "Land-line Phone Model LL321",
                        DateCreate = new DateTime(2024,1,10),
                        IsResponse = true,
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
                        RegionId = 1,
                        CustomerId = 2
                    },
                    new CustomerRequest
                    {
                        RequestTitle = "Broadband Router Replacement",
                        ServiceRequest = "The broadband router is not working and needs replacement.",
                        EquipmentRequest = "New Router Model BR999",
                        DateCreate = new DateTime(2024,1,10),
                        IsResponse = true,
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
                        RegionId = 1,
                        CustomerId = 3
                    },
                    new CustomerRequest
                    {
                        RequestTitle = "Dial-up Connection Issue",
                        ServiceRequest = "Frequent disconnections while using dial-up internet.",
                        EquipmentRequest = "Replacement Modem Model DM111",
                        DateCreate = new DateTime(2024,1,10),
                        IsResponse = true,
                        RegionId = 1,
                        CustomerId = 3
                    },
                    new CustomerRequest
                    {
                        RequestTitle = "test",
                        ServiceRequest = "test",
                        EquipmentRequest = "test",
                        DateCreate = new DateTime(2024,1,10),
                        IsResponse = true,
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
                        CustomerId = 1, // Replace with valid CustomerId
                        EmpIdResolver = null // No resolver yet
                    },
                    new SupportRequest
                    {
                        DateRequest = new DateTime(2024, 1, 5),
                        Title = "Request for Broadband Repair",
                        DetailContent = "The broadband connection is not working since yesterday evening.",
                        DateResolved = null,
                        IsResolved = false,
                        CustomerId = 1, // Replace with valid CustomerId
                        EmpIdResolver = null // No resolver yet
                    },
                    new SupportRequest
                    {
                        DateRequest = new DateTime(2024, 1, 10),
                        Title = "Request for Land-line Repair",
                        DetailContent = "Land-line phone is not working properly; there is a lot of static noise.",
                        DateResolved = new DateTime(2024, 1, 12),
                        IsResolved = true,
                        CustomerId = 2, // Replace with valid CustomerId
                        EmpIdResolver = 1 // EmployeeId of resolver
                    },
                    new SupportRequest
                    {
                        DateRequest = new DateTime(2024, 1, 15),
                        Title = "Help with Dial-up Internet Configuration",
                        DetailContent = "Need assistance in configuring the dial-up connection for better speed.",
                        DateResolved = null,
                        IsResolved = false,
                        CustomerId = 3, // Replace with valid CustomerId
                        EmpIdResolver = null // No resolver yet
                    },
                };
                _dbContext.SupportRequests.AddRange(supportRequests);
                _dbContext.SaveChanges();
            }
            // Seed FeedBacks
            if (!_dbContext.FeedBacks.Any())
            {
                var feedbacks = new List<FeedBack>
                {
                    new FeedBack
                    {
                        Date = new DateTime(2024, 1, 5),
                        Title = "test",
                        FeedBackContent = "test",
                        Status = true, // Feedback has been read
                        CustomerId = 1 // Replace with valid CustomerId
                    },
                    new FeedBack
                    {
                        Date = new DateTime(2024, 1, 6),
                        Title = "Excellent Service",
                        FeedBackContent = "The technician was very helpful and resolved my issue quickly.",
                        Status = true, // Feedback has been read
                        CustomerId = 1 // Replace with valid CustomerId
                    },
                    new FeedBack
                    {
                        Date = new DateTime(2024, 1, 6),
                        Title = "Need Faster Response",
                        FeedBackContent = "The service team took longer than expected to respond to my request.",
                        Status = false, // Feedback not yet read
                        CustomerId = 2 // Replace with valid CustomerId
                    },
                    new FeedBack
                    {
                        Date = new DateTime(2024, 1, 7),
                        Title = "Great Support",
                        FeedBackContent = "I appreciate the support I received for setting up my new broadband connection.",
                        Status = true, // Feedback has been read
                        CustomerId = 3 // Replace with valid CustomerId
                    }
                };

                _dbContext.FeedBacks.AddRange(feedbacks);
                _dbContext.SaveChanges();
            }
        }
    }
}
