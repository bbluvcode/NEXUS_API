using NEXUS_API.Models;

namespace NEXUS_API.Repository
{
    public interface IRetainShopRepository
    {
        Task<IEnumerable<RetainShop>> GetAllRetainShopsAsync();
        Task<RetainShop> GetRetainShopByIdAsync(int id);
        Task AddRetainShopAsync(RetainShop retainShop);
        Task UpdateRetainShopAsync(RetainShop retainShop);
    }
}
