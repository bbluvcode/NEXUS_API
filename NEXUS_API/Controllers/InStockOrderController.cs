using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Collections.Generic;
using NEXUS_API.Models;
using NEXUS_API.Data;
using NEXUS_API.Helpers;

namespace NEXUS_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InStockOrderController : ControllerBase
    {
        private readonly DatabaseContext _dbContext;

        public InStockOrderController(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetInStockOrders()
        {
            var orders = await _dbContext.InStockOrders
                .Include(o => o.InStockRequest)
                .Include(o => o.Vendor)
                .Include(o => o.Employee)
                .Include(o => o.Stock)
                .Include(o => o.InStockOrderDetails)
                .ToListAsync();

            return Ok(new ApiResponse(200, "Get in-stock orders successfully", orders));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetInStockOrder(int id)
        {
            var order = await _dbContext.InStockOrders
                .Include(o => o.InStockRequest)
                .Include(o => o.Vendor)
                .Include(o => o.Employee)
                .Include(o => o.Stock)
                .Include(o => o.InStockOrderDetails)
                .FirstOrDefaultAsync(o => o.InStockOrderId == id);

            if (order == null)
                return NotFound(new ApiResponse(404, "In-stock order not found", null));

            return Ok(new ApiResponse(200, "Get in-stock order successfully", order));
        }

        [HttpPost]
        public async Task<IActionResult> CreateInStockOrder([FromBody] InStockOrder order)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse(400, "Invalid data", null));

            _dbContext.InStockOrders.Add(order);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetInStockOrder), new { id = order.InStockOrderId }, new ApiResponse(201, "Created in-stock order successfully", order));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateInStockOrder(int id, [FromBody] InStockOrder orderUpdate)
        {
            var order = await _dbContext.InStockOrders.FindAsync(id);
            if (order == null)
                return NotFound(new ApiResponse(404, "In-stock order not found", null));

            order.VendorId = orderUpdate.VendorId;
            order.EmployeeId = orderUpdate.EmployeeId;
            order.StockId = orderUpdate.StockId;
            order.Payer = orderUpdate.Payer;
            order.CreateDate = orderUpdate.CreateDate;
            order.InstockDate = orderUpdate.InstockDate;
            order.PayDate = orderUpdate.PayDate;
            order.Tax = orderUpdate.Tax;
            order.Total = orderUpdate.Total;
            order.CurrencyUnit = orderUpdate.CurrencyUnit;
            order.isPay = orderUpdate.isPay;

            await _dbContext.SaveChangesAsync();
            return Ok(new ApiResponse(200, "Updated in-stock order successfully", order));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInStockOrder(int id)
        {
            var order = await _dbContext.InStockOrders.FindAsync(id);
            if (order == null)
                return NotFound(new ApiResponse(404, "In-stock order not found", null));

            _dbContext.InStockOrders.Remove(order);
            await _dbContext.SaveChangesAsync();

            return Ok(new ApiResponse(200, "Deleted in-stock order successfully", order));
        }
    }
}
