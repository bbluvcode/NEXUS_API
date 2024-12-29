using NEXUS_API.Models;

namespace NEXUS_API.Repository
{
    public interface IRegionRepository
    {
        Task<IEnumerable<Region>> GetAllRegionsAsync();
        Task<Region> GetRegionByIdAsync(int id);
        Task AddRegionAsync(Region region);
        Task UpdateRegionAsync(Region region);
    }
}
