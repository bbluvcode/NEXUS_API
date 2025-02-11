using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using NEXUS_API.Models;
using NEXUS_API.Data;
using NEXUS_API.Helpers;

namespace NEXUS_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly DatabaseContext _dbContext;

        public StockController(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetStocks()
        {
            try
            {
                var stocks = await _dbContext.Stocks
                    .AsNoTracking()
                    .Include(s => s.Region)
                    .Include(s => s.Equipments)
                    .Include(s => s.InStockOrders)
                    .Include(s => s.OutStockOrders)
                    .ToListAsync();

                return Ok(new ApiResponse(StatusCodes.Status200OK, "Get stocks successfully", stocks));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(StatusCodes.Status500InternalServerError, $"Server error: {ex.Message}", null));
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateStock([FromBody] Stock stock)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Invalid data", null));
            }

            try
            {
                await _dbContext.Stocks.AddAsync(stock);
                await _dbContext.SaveChangesAsync();
                return CreatedAtAction(nameof(GetStocks), new { id = stock.StockId }, new ApiResponse(StatusCodes.Status201Created, "Stock created successfully", stock));
            }
            catch (DbUpdateException ex)
            {
                return Conflict(new ApiResponse(StatusCodes.Status409Conflict, $"Database error: {ex.Message}", null));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(StatusCodes.Status500InternalServerError, $"Server error: {ex.Message}", null));
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStock(int id, [FromBody] Stock stockUpdate)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Invalid data", null));
            }

            var stock = await _dbContext.Stocks.FindAsync(id);
            if (stock == null)
            {
                return NotFound(new ApiResponse(StatusCodes.Status404NotFound, "Stock not found", null));
            }

            try
            {
                stock.StockName = stockUpdate.StockName;
                stock.Address = stockUpdate.Address;
                stock.Email = stockUpdate.Email;
                stock.Phone = stockUpdate.Phone;
                stock.Fax = stockUpdate.Fax;
                stock.RegionId = stockUpdate.RegionId;

                await _dbContext.SaveChangesAsync();
                return Ok(new ApiResponse(StatusCodes.Status200OK, "Stock updated successfully", stock));
            }
            catch (DbUpdateException ex)
            {
                return Conflict(new ApiResponse(StatusCodes.Status409Conflict, $"Database update error: {ex.Message}", null));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(StatusCodes.Status500InternalServerError, $"Server error: {ex.Message}", null));
            }
        }
    }
}
