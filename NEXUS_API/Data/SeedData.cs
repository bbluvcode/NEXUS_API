using Microsoft.EntityFrameworkCore;
using NEXUS_API.Models;

namespace NEXUS_API.Data
{
    public class SeedData
    {
        public static void SeedingData(DatabaseContext _dbContext)
        {
            _dbContext.Database.Migrate();

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
                     
        }

    }
}
