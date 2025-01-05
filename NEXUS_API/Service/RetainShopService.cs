    using Microsoft.EntityFrameworkCore;
    using NEXUS_API.Data;
    using NEXUS_API.Models;
    using NEXUS_API.Repository;

    namespace NEXUS_API.Service
    {
        public class RetainShopService : IRetainShopRepository
        {
            private readonly DatabaseContext _dbContext;
            public RetainShopService(DatabaseContext dbContext)
            {
                _dbContext = dbContext;
            }
            public async Task AddRetainShopAsync(RetainShop retainShop)
            {
                _dbContext.RetainShops.Add(retainShop);
                await _dbContext.SaveChangesAsync();
            }

            public async Task<IEnumerable<RetainShop>> GetAllRetainShopsAsync()
            {
                return await _dbContext.RetainShops.ToListAsync();
            }

            public async Task<RetainShop> GetRetainShopByIdAsync(int id)
            {
                return await _dbContext.RetainShops.FirstOrDefaultAsync(r => r.RetainShopId == id);
            }

        public async Task UpdateRetainShopAsync(RetainShop retainShop)
        {
            // Tìm kiếm RetainShop hiện tại trong cơ sở dữ liệu
            var existingRetainShop = await _dbContext.RetainShops.FindAsync(retainShop.RetainShopId);
            if (existingRetainShop != null)
            {
                // Cập nhật tất cả các thuộc tính của RetainShop
                existingRetainShop.RetainShopName = retainShop.RetainShopName;
                existingRetainShop.Address = retainShop.Address;
                existingRetainShop.Email = retainShop.Email;
                existingRetainShop.Phone = retainShop.Phone;
                existingRetainShop.IsMainOffice = retainShop.IsMainOffice;
                existingRetainShop.Fax = retainShop.Fax;
                existingRetainShop.RegionId = retainShop.RegionId;

                // Lưu các thay đổi vào cơ sở dữ liệu
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                // Nếu không tìm thấy RetainShop, ném lỗi
                throw new KeyNotFoundException($"RetainShop with ID {retainShop.RetainShopId} not found.");
            }
        }
    }
}
