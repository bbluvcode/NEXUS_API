using Microsoft.EntityFrameworkCore;
using NEXUS_API.Data;
using NEXUS_API.Models;
using NEXUS_API.Repository;

namespace NEXUS_API.Service
{
    public class RetailShopService : IRetailShopRepository
    {
        private readonly DatabaseContext _dbContext;

        public RetailShopService(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddRetailShopAsync(RetailShop retailShop)
        {
            await _dbContext.RetailShops.AddAsync(retailShop);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<RetailShop>> GetAllRetailShopsAsync()
        {
            return await _dbContext.RetailShops.ToListAsync();
        }

        public async Task<RetailShop> GetRetailShopByIdAsync(int id)
        {
            return await _dbContext.RetailShops.FirstOrDefaultAsync(r => r.RetailShopId == id);
        }

        public async Task UpdateRetailShopAsync(RetailShop retailShop)
        {
            var existingRetailShop = await _dbContext.RetailShops.FindAsync(retailShop.RetailShopId);
            if (existingRetailShop != null)
            {
                existingRetailShop.RetailShopName = retailShop.RetailShopName;
                existingRetailShop.Address = retailShop.Address;
                existingRetailShop.Email = retailShop.Email;
                existingRetailShop.Phone = retailShop.Phone;
                existingRetailShop.IsMainOffice = retailShop.IsMainOffice;
                existingRetailShop.Fax = retailShop.Fax;
                existingRetailShop.RegionId = retailShop.RegionId;
                if (!string.IsNullOrEmpty(retailShop.Image)) // Chỉ cập nhật ảnh nếu có thay đổi
                {
                    existingRetailShop.Image = retailShop.Image;
                }

                await _dbContext.SaveChangesAsync();
            }
            else
            {
                throw new KeyNotFoundException($"RetailShop with ID {retailShop.RetailShopId} not found.");
            }
        }
    }
}
