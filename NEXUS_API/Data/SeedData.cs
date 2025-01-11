using Microsoft.EntityFrameworkCore;
using NEXUS_API.Models;

namespace NEXUS_API.Data
{
    public class SeedData
    {
        public static void SeedingData(DatabaseContext _dbContext)
        {
            _dbContext.Database.Migrate();
            // Seed Regions
            if (!_dbContext.Regions.Any())
            {
                var regions = new List<Region>
                {
                   new Region
                    {
                        RegionCode = "HCMC-001",
                        RegionName = "Ho Chi Minh City"
                    },
                    new Region
                    {
                        RegionCode = "HN-002",
                        RegionName = "Ha Noi"
                    },
                    new Region
                    {
                        RegionCode = "DN-003",
                        RegionName = "Da Nang"
                    },
                    new Region
                    {
                        RegionCode = "ASIA-004",
                        RegionName = "Asia Headquarters"
                    },
                    new Region
                    {
                        RegionCode = "SG-005",
                        RegionName = "Singapore"
                    },
                    new Region
                    {
                        RegionCode = "KL-006",
                        RegionName = "Kuala Lumpur"
                    },
                    new Region
                    {
                        RegionCode = "BKK-007",
                        RegionName = "Bangkok"
                    }

                };
                _dbContext.Regions.AddRange(regions);
                _dbContext.SaveChanges();
            }
            // Seed EmployeeRoles
            if (!_dbContext.EmployeeRoles.Any())
            {
                var roles = new List<EmployeeRole>
                {
                    new EmployeeRole { RoleName = "Admin" },
                    new EmployeeRole { RoleName = "Technical" },
                    new EmployeeRole { RoleName = "Accountant" }
                };
                _dbContext.EmployeeRoles.AddRange(roles);
                _dbContext.SaveChanges();
            }

            // Seed RetailShops
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
                        RegionId = 1 // Replace with a valid RegionId in your database
                    },
                    new RetailShop
                    {
                        RetailShopName = "Branch A",
                        Address = "456 Branch St, District 3, HCMC",
                        Email = "brancha@nexus.com",
                        Phone = "0987654321",
                        IsMainOffice = false,
                        Fax = "987654321",
                        RegionId = 1 // Replace with a valid RegionId in your database
                    },
                    new RetailShop
                    {
                        RetailShopName = "Branch B",
                        Address = "789 Second Ave, District 7, HCMC",
                        Email = "branchb@nexus.com",
                        Phone = "0912345678",
                        IsMainOffice = false,
                        Fax = "091234567",
                        RegionId = 2 // Replace with a valid RegionId in your database
                    },
                    new RetailShop
                    {
                        RetailShopName = "Asia HQ",
                        Address = "789 Asia Center, Singapore",
                        Email = "asiahq@nexus.com",
                        Phone = "0923456789",
                        IsMainOffice = false,
                        Fax = "092345678",
                        RegionId = 4 // Asia Headquarters
                    },
                    new RetailShop
                    {
                        RetailShopName = "Singapore Branch",
                        Address = "123 Orchard Rd, Singapore",
                        Email = "sgbranch@nexus.com",
                        Phone = "0911122233",
                        IsMainOffice = false,
                        Fax = "0911122233",
                        RegionId = 5 // Singapore
                    },
                    new RetailShop
                    {
                        RetailShopName = "Kuala Lumpur Branch",
                        Address = "45 Jalan Ampang, Kuala Lumpur",
                        Email = "klbranch@nexus.com",
                        Phone = "0909988776",
                        IsMainOffice = false,
                        Fax = "0909988776",
                        RegionId = 6 // Kuala Lumpur
                    },
                    new RetailShop
                    {
                        RetailShopName = "Bangkok Branch",
                        Address = "99 Sukhumvit Rd, Bangkok",
                        Email = "bkkbranch@nexus.com",
                        Phone = "0988765432",
                        IsMainOffice = false,
                        Fax = "0988765432",
                        RegionId = 7 // Bangkok
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
            if (!_dbContext.CustomerRequests.Any())
            {
                var customerRequests = new List<CustomerRequest>
                {
                    new CustomerRequest
                    {
                        RequestTitle = "Broadband Installation",
                        ServiceRequest = "Request to install a broadband internet connection.",
                        EquipmentRequest = "Broadband Router Model BR123",
                        IsResponse = false,
                        CustomerId = 1
                    },
                    new CustomerRequest
                    {
                        RequestTitle = "Land-line Issue",
                        ServiceRequest = "The land-line phone has no dial tone.",
                        EquipmentRequest = "Replacement Handset Model LH456",
                        IsResponse = false,
                        CustomerId = 1
                    },
                    new CustomerRequest
                    {
                        RequestTitle = "Dial-up Assistance",
                        ServiceRequest = "Unable to connect to the internet using dial-up.",
                        EquipmentRequest = "Dial-up Modem Model DM789",
                        IsResponse = true,
                        CustomerId = 1
                    },
                    new CustomerRequest
                    {
                        RequestTitle = "Broadband Speed Issue",
                        ServiceRequest = "Broadband connection is slower than expected.",
                        EquipmentRequest = null,
                        IsResponse = false,
                        CustomerId = 2
                    },
                    new CustomerRequest
                    {
                        RequestTitle = "Land-line Installation",
                        ServiceRequest = "Request to install a new land-line phone.",
                        EquipmentRequest = "Land-line Phone Model LL321",
                        IsResponse = true,
                        CustomerId = 2
                    },
                    new CustomerRequest
                    {
                        RequestTitle = "Dial-up Account Setup",
                        ServiceRequest = "Need help setting up a new dial-up account.",
                        EquipmentRequest = null,
                        IsResponse = false,
                        CustomerId = 2
                    },
                    new CustomerRequest
                    {
                        RequestTitle = "Broadband Router Replacement",
                        ServiceRequest = "The broadband router is not working and needs replacement.",
                        EquipmentRequest = "New Router Model BR999",
                        IsResponse = true,
                        CustomerId = 3
                    },
                    new CustomerRequest
                    {
                        RequestTitle = "Land-line Noise Issue",
                        ServiceRequest = "There is excessive noise on the land-line during calls.",
                        EquipmentRequest = null,
                        IsResponse = false,
                        CustomerId = 3
                    },
                    new CustomerRequest
                    {
                        RequestTitle = "Dial-up Connection Issue",
                        ServiceRequest = "Frequent disconnections while using dial-up internet.",
                        EquipmentRequest = "Replacement Modem Model DM111",
                        IsResponse = true,
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
                    }
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
                        Title = "Excellent Service",
                        FeedBackContent = "The technician was very helpful and resolved my issue quickly.",
                        Status = true, // Feedback has been read
                        CustomerId = 1 // Replace with valid CustomerId
                    },
                    new FeedBack
                    {
                        Title = "Need Faster Response",
                        FeedBackContent = "The service team took longer than expected to respond to my request.",
                        Status = false, // Feedback not yet read
                        CustomerId = 2 // Replace with valid CustomerId
                    },
                    new FeedBack
                    {
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
