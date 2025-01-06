using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
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
            var stocks = await _dbContext.Stocks
                .Include(s => s.RetailShop)
                .Include(s => s.Region)
                .Include(s => s.Equipments)
                .Include(s => s.InStockOrders)
                .Include(s => s.OutStockOrders)
                .ToListAsync();

            var response = new ApiResponse(StatusCodes.Status200OK, "get stocks successfully", stocks);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateStock([FromBody] Stock stock)
        {
            object response = null;
            try
            {
                if (ModelState.IsValid)
                {
                    await _dbContext.Stocks.AddAsync(stock);
                    await _dbContext.SaveChangesAsync();
                    response = new ApiResponse(StatusCodes.Status201Created, "create stock successfully", stock);
                    return Created("success", response);
                }

                response = new ApiResponse(StatusCodes.Status400BadRequest, "bad request", null);
                return BadRequest(response);
            }
            catch (Exception)
            {
                response = new ApiResponse(StatusCodes.Status500InternalServerError, "server error", null);
                return StatusCode(500, response);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStock(int id, [FromBody] Stock stockUpdate)
        {
            object response = null;
            try
            {
                var stock = await _dbContext.Stocks.FirstOrDefaultAsync(s => s.StockId == id);
                if (stock == null)
                {
                    response = new ApiResponse(StatusCodes.Status404NotFound, "stock not found", null);
                    return NotFound(response);
                }

                if (!ModelState.IsValid)
                {
                    response = new ApiResponse(StatusCodes.Status400BadRequest, "bad request", null);
                    return BadRequest(response);
                }

                stock.StockName = stockUpdate.StockName;
                stock.Address = stockUpdate.Address;
                stock.Email = stockUpdate.Email;
                stock.Phone = stockUpdate.Phone;
                stock.Fax = stockUpdate.Fax;
                stock.RetailShopId = stockUpdate.RetailShopId;
                stock.RegionId = stockUpdate.RegionId;

                await _dbContext.SaveChangesAsync();
                response = new ApiResponse(StatusCodes.Status200OK, "update stock successfully", stock);
                return Ok(response);
            }
            catch (Exception)
            {
                response = new ApiResponse(StatusCodes.Status500InternalServerError, "server error", null);
                return StatusCode(500, response);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStock(int id)
        {
            object response = null;
            try
            {
                var stock = await _dbContext.Stocks.FirstOrDefaultAsync(s => s.StockId == id);
                if (stock == null)
                {
                    response = new ApiResponse(StatusCodes.Status404NotFound, "stock not found", null);
                    return NotFound(response);
                }

                _dbContext.Stocks.Remove(stock);
                await _dbContext.SaveChangesAsync();
                response = new ApiResponse(StatusCodes.Status200OK, "delete stock successfully", stock);
                return Ok(response);
            }
            catch (Exception)
            {
                response = new ApiResponse(StatusCodes.Status500InternalServerError, "server error", null);
                return StatusCode(500, response);
            }
        }
    }
}
