using Microsoft.EntityFrameworkCore;
using NEXUS_API.Data;
using NEXUS_API.Models;
using NEXUS_API.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NEXUS_API.Service
{
    public class VendorService : IVendorRepository
    {
        private readonly DatabaseContext _dbContext;

        public VendorService(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Vendor>> GetAllVendorsAsync()
        {
            return await _dbContext.Vendors.ToListAsync();
        }

        public async Task<Vendor> GetVendorByIdAsync(int id)
        {
            return await _dbContext.Vendors.FirstOrDefaultAsync(v => v.VendorId == id);
        }

        public async Task AddVendorAsync(Vendor vendor)
        {
            await _dbContext.Vendors.AddAsync(vendor);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateVendorAsync(Vendor vendor)
        {
            var existingVendor = await _dbContext.Vendors.FindAsync(vendor.VendorId);
            if (existingVendor != null)
            {
                existingVendor.VendorName = vendor.VendorName;
                existingVendor.Address = vendor.Address;
                existingVendor.Email = vendor.Email;
                existingVendor.Phone = vendor.Phone;
                existingVendor.Fax = vendor.Fax;
                existingVendor.Description = vendor.Description;
                existingVendor.RegionId = vendor.RegionId;
                existingVendor.Status = vendor.Status;

                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task UpdateVendorStatusAsync(int id, bool status)
        {
            var vendor = await _dbContext.Vendors.FindAsync(id);
            if (vendor != null)
            {
                vendor.Status = status;
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                throw new KeyNotFoundException($"Vendor with ID {id} not found.");
            }
        }
    }
}
