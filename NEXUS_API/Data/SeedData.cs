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
                        Password = "password123"
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
                        Password = "password456"
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
                        Password = "password789"
                    }
                );

                _dbContext.SaveChanges();
            }


            //// Seed dữ liệu Categories
            //if (!_dbContext.Categories.Any())
            //{
            //    _dbContext.Categories.AddRange(
            //        new Category { Name = "Electronics", Status = true },
            //        new Category { Name = "Clothing", Status = true },
            //        new Category { Name = "Books", Status = true }
            //    );
            //    _dbContext.SaveChanges();  // Lưu sản phẩm vào cơ sở dữ liệu trước khi thêm ảnh
            //}

            //// Seed dữ liệu Products
            //if (!_dbContext.Products.Any())
            //{
            //    _dbContext.Products.AddRange(
            //        new Product
            //        {
            //            Name = "IPHONE 16",
            //            Price = 35000000.00m,
            //            Quantity = 10,
            //            Status = true,
            //            CategoryId = 1
            //        },
            //        new Product
            //        {
            //            Name = "Áo sơ mi",
            //            Price = 12000.00m,
            //            Quantity = 10,
            //            Status = true,
            //            CategoryId = 2
            //        },
            //        new Product
            //        {
            //            Name = "Nhắm mắt mở cửa sổ",
            //            Price = 200000.00m,
            //            Quantity = 10,
            //            Status = true,
            //            CategoryId = 3
            //        }
            //    );
            //    _dbContext.SaveChanges();  // Lưu sản phẩm vào cơ sở dữ liệu trước khi thêm ảnh
            //}

            //// Seed dữ liệu ProductImages
            //if (!_dbContext.ProductImages.Any())
            //{
            //    _dbContext.ProductImages.AddRange(
            //        new ProductImage
            //        {
            //            ImageUrl = "http://localhost:5185/Uploads/productImages/iPhone-16-Proi.png",
            //            ProductId = 1
            //        },
            //        new ProductImage
            //        {
            //            ImageUrl = "http://localhost:5185/Uploads/productImages/Tshirt.jpg",
            //            ProductId = 2
            //        },
            //        new ProductImage
            //        {
            //            ImageUrl = "http://localhost:5185/Uploads/productImages/book.jpg",
            //            ProductId = 3
            //        }
            //    );
            //    _dbContext.SaveChanges();
            //}

            //// Seed dữ liệu Users
            //if (!_dbContext.Users.Any())
            //{
            //    _dbContext.Users.AddRange(
            //         new User
            //         {
            //             FullName = "Admin User",
            //             Email = "admin@gmail.com",
            //             Password = "123",
            //             Phones = "123456789",
            //             DOB = null,
            //             Role = "ADMIN",
            //             ImageUrl = null,
            //             Status = true,
            //             FailedLoginAttempts = 0,
            //             Expired = null,
            //             TOKEN = null,
            //             TokenIsUsed = null,
            //             ExpiredTOKEN = null
            //         },
            //         new User
            //         {
            //             FullName = "Regular User",
            //             Email = "user@gmail.com",
            //             Password = "123",
            //             Phones = "987654321",
            //             DOB = null,
            //             Role = "USER",
            //             ImageUrl = null,
            //             Status = true,
            //             FailedLoginAttempts = 0,
            //             Expired = null,
            //             TOKEN = null,
            //             TokenIsUsed = null,
            //             ExpiredTOKEN = null
            //         }
            //     );
            //    _dbContext.SaveChanges();
            //}

            //// Seed dữ liệu UserProducts (mối quan hệ giữa User và Product)
            //if (!_dbContext.UserProducts.Any())
            //{
            //    var admin = _dbContext.Users.FirstOrDefault(u => u.Email == "admin@gmail.com");
            //    var regularUser = _dbContext.Users.FirstOrDefault(u => u.Email == "user@gmail.com");
            //    var iphone16 = _dbContext.Products.FirstOrDefault(p => p.Name == "IPHONE 16");
            //    var tshirt = _dbContext.Products.FirstOrDefault(p => p.Name == "Áo sơ mi");

            //    if (admin != null && iphone16 != null)
            //    {
            //        _dbContext.UserProducts.Add(new UserProduct
            //        {
            //            UserId = admin.Id,
            //            ProductId = iphone16.Id,
            //            Quantity = 2
            //        });
            //    }

            //    if (regularUser != null && tshirt != null)
            //    {
            //        _dbContext.UserProducts.Add(new UserProduct
            //        {
            //            UserId = regularUser.Id,
            //            ProductId = tshirt.Id,
            //            Quantity = 3
            //        });
            //    }
            //}

            //_dbContext.SaveChanges();
        }

    }
}
