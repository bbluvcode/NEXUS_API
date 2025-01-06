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
                _dbContext.RetailShops.Add(retailShop);
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
            // Tìm kiếm RetailShop hiện tại trong cơ sở dữ liệu
            var existingRetailShop = await _dbContext.RetailShops.FindAsync(retailShop.RetailShopId);
            if (existingRetailShop != null)
            {
                // Cập nhật tất cả các thuộc tính của RetailShop
                existingRetailShop.RetailShopName = retailShop.RetailShopName;
                existingRetailShop.Address = retailShop.Address;
                existingRetailShop.Email = retailShop.Email;
                existingRetailShop.Phone = retailShop.Phone;
                existingRetailShop.IsMainOffice = retailShop.IsMainOffice;
                existingRetailShop.Fax = retailShop.Fax;
                existingRetailShop.RegionId = retailShop.RegionId;

                // Lưu các thay đổi vào cơ sở dữ liệu
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                // Nếu không tìm thấy RetailShop, ném lỗi
                throw new KeyNotFoundException($"RetailShop with ID {retailShop.RetailShopId} not found.");
            }
        }
    }
}
