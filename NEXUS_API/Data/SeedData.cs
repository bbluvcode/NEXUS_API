using Microsoft.EntityFrameworkCore;

namespace NEXUS_API.Data
{
    public class SeedData
    {
        public static void SeedingData(DatabaseContext _dbContext)
        {
            _dbContext.Database.Migrate();

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
            //            ImageUrl = "http://localhost:5031/Uploads/productImages/iPhone-16-Proi.png",
            //            ProductId = 1
            //        },
            //        new ProductImage
            //        {
            //            ImageUrl = "http://localhost:5031/Uploads/productImages/Tshirt.jpg",
            //            ProductId = 2
            //        },
            //        new ProductImage
            //        {
            //            ImageUrl = "http://localhost:5031/Uploads/productImages/book.jpg",
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
