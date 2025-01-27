using NEXUS_API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NEXUS_API.Repository
{
    public interface IVendorRepository
    {
        Task<IEnumerable<Vendor>> GetAllVendorsAsync();
        Task<Vendor?> GetVendorByIdAsync(int id);
        Task AddVendorAsync(Vendor vendor);
        Task UpdateVendorAsync(Vendor vendor);
        Task UpdateVendorStatusAsync(int id, bool status);
    }
}