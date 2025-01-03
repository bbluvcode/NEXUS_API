using Microsoft.EntityFrameworkCore;
using NEXUS_API.Data;
using NEXUS_API.Models;
using NEXUS_API.Repository;

namespace NEXUS_API.Service
{
    public class RegionService : IRegionRepository
    {
        private readonly DatabaseContext _dbContext;
        public RegionService(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Region>> GetAllRegionsAsync()
        {
            return await _dbContext.Regions.ToListAsync();
        }

        public async Task<Region> GetRegionByIdAsync(int id)
        {
            return await _dbContext.Regions.FirstOrDefaultAsync(r => r.RegionId == id);
        }

        public async Task AddRegionAsync(Region region)
        {
            _dbContext.Regions.Add(region);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateRegionAsync(Region region)
        {
            var existingRegion = await _dbContext.Regions.FindAsync(region.RegionId);
            if (existingRegion != null)
            {
                existingRegion.RegionCode = region.RegionCode;
                existingRegion.RegionName = region.RegionName;

                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
