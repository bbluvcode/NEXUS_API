using NEXUS_API.Models;

namespace NEXUS_API.Repository
{
    public interface IRetailShopRepository
    {
        Task<IEnumerable<RetailShop>> GetAllRetailShopsAsync();
        Task<RetailShop> GetRetailShopByIdAsync(int id);
        Task AddRetailShopAsync(RetailShop retailShop);
        Task UpdateRetailShopAsync(RetailShop retailShop);
    }
}
