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
    public class OutStockOrderController : ControllerBase
    {
        private readonly DatabaseContext _dbContext;

        public OutStockOrderController(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetOutStockOrders()
        {
            var orders = await _dbContext.OutStockOrders
                .Include(o => o.Stock)
                .Include(o => o.Employee)
                .Include(o => o.OutStockOrderDetails)
                .ToListAsync();

            return Ok(new ApiResponse(200, "Get out-stock orders successfully", orders));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOutStockOrder(int id)
        {
            var order = await _dbContext.OutStockOrders
                .Include(o => o.Stock)
                .Include(o => o.Employee)
                .Include(o => o.OutStockOrderDetails)
                .FirstOrDefaultAsync(o => o.OutStockId == id);

            if (order == null)
                return NotFound(new ApiResponse(404, "Out-stock order not found", null));

            return Ok(new ApiResponse(200, "Get out-stock order successfully", order));
        }

        [HttpPost]
        public async Task<IActionResult> CreateOutStockOrder([FromBody] OutStockOrder order)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse(400, "Invalid data", null));

            _dbContext.OutStockOrders.Add(order);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetOutStockOrder), new { id = order.OutStockId }, new ApiResponse(201, "Created out-stock order successfully", order));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOutStockOrder(int id, [FromBody] OutStockOrder orderUpdate)
        {
            var order = await _dbContext.OutStockOrders.FindAsync(id);
            if (order == null)
                return NotFound(new ApiResponse(404, "Out-stock order not found", null));

            order.StockId = orderUpdate.StockId;
            order.EmployeeId = orderUpdate.EmployeeId;
            order.CreateDate = orderUpdate.CreateDate;
            order.PayDate = orderUpdate.PayDate;
            order.Tax = orderUpdate.Tax;
            order.Total = orderUpdate.Total;
            order.isPay = orderUpdate.isPay;

            await _dbContext.SaveChangesAsync();
            return Ok(new ApiResponse(200, "Updated out-stock order successfully", order));
        }

        
    }
}
