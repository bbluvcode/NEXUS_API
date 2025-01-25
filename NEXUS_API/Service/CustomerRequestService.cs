using NEXUS_API.Data;
using NEXUS_API.Models;

namespace NEXUS_API.Service
{
    public class CustomerRequestService
    {
        private readonly DatabaseContext _context;
        public CustomerRequestService(DatabaseContext context) { _context = context; }
        //public async Task<PayPalCustomerRequestDeposit> CreateCustomerRequest

    }
}
