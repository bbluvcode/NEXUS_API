using Microsoft.EntityFrameworkCore;
using NEXUS_API.Data;
using NEXUS_API.Models;

namespace NEXUS_API.Service
{
    public class OrderService
    {
        private readonly DatabaseContext _dbContext;

        public OrderService(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<ServiceOrder> CreateOrderFromRequestAsync(CustomerRequest customerRequest)
        {
            string orderId = GenerateOrderId(customerRequest);

            var serviceOrder = new ServiceOrder
            {
                OrderId = orderId,
                RequestId = customerRequest.RequestId,
                DateCreate = DateTime.UtcNow,
                Deposit = 0 
            };

            _dbContext.ServiceOrders.Add(serviceOrder);
            await _dbContext.SaveChangesAsync();

            return serviceOrder;
        }

        private string GenerateOrderId(CustomerRequest customerRequest)
        {
            char connectionType = DetermineConnectionType(customerRequest.ServiceRequest);
            int nextSerialNumber = GetNextSerialNumber();
            return $"{connectionType}{nextSerialNumber.ToString("D10")}";
        }

        private char DetermineConnectionType(string serviceRequest)
        {
            if (serviceRequest.Contains("Dial-Up"))
            {
                return 'D';
            }
            else if (serviceRequest.Contains("Broadband"))
            {
                return 'B';
            }
            else if (serviceRequest.Contains("Telephone"))
            {
                return 'T';
            }
            else
            {
                throw new ArgumentException("Invalid service request type.");
            }
        }
        private int GetNextSerialNumber()
        {
            return _dbContext.Database.ExecuteSqlRaw("SELECT NEXT VALUE FOR OrderSerialNumber");
        }
    }
}

