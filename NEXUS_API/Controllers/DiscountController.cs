using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NEXUS_API.Data;
using NEXUS_API.Helpers;
using NEXUS_API.Models;

namespace NEXUS_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiscountController : ControllerBase
    {
        private readonly DatabaseContext _dbContext;
        public DiscountController(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllDiscounts()
        {
            try
            {
                var discounts = await _dbContext.Discounts.ToListAsync();
                var response = new ApiResponse(StatusCodes.Status200OK, "Get all successfully", discounts);
                return Ok(response);
            }
            catch (Exception ex)
            {
                var response = new ApiResponse(StatusCodes.Status500InternalServerError, "Internal Server Error", null);
                return StatusCode(500, response);
            }
        }
        [HttpPost]
        public async Task<IActionResult> AddDiscount([FromBody] Discount Discount)
        {
            object response = null;
            try
            {
                await _dbContext.Discounts.AddAsync(Discount);
                await _dbContext.SaveChangesAsync();
                response = new ApiResponse(StatusCodes.Status201Created, "Discount created successfully", Discount);
                return Created("success", response);
            }
            catch (Exception ex)
            {
                response = new ApiResponse(StatusCodes.Status500InternalServerError, "Internal Server Error", null);
                return new ObjectResult(response);
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDiscount(int id, [FromBody] Discount Discount)
        {
            object response = null;
            try
            {
                var DiscountExisting = await _dbContext.Discounts.FindAsync(id);
                if (DiscountExisting == null)
                {
                    response = new ApiResponse(StatusCodes.Status404NotFound, "Discount not found", null);
                    return NotFound(response);
                }
                DiscountExisting.DiscountName = Discount.DiscountName;
                DiscountExisting.StartDate = Discount.StartDate;
                DiscountExisting.EndDate = Discount.EndDate;
                DiscountExisting.DiscountPercentage = Discount.DiscountPercentage;
                await _dbContext.SaveChangesAsync();
                response = new ApiResponse(StatusCodes.Status200OK, "Discount updated successfully", DiscountExisting);
                return Ok(response);
            }
            catch (Exception ex)
            {
                response = new ApiResponse(StatusCodes.Status500InternalServerError, "Internal server error", null);
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDiscount(int id)
        {
            object response = null;
            try
            {
                var DiscountExisting = await _dbContext.Discounts.FindAsync(id);
                if (DiscountExisting == null)
                {
                    response = new ApiResponse(StatusCodes.Status404NotFound, "Discount not found", null);
                    return NotFound(response);
                }
                _dbContext.Discounts.Remove(DiscountExisting);
                await _dbContext.SaveChangesAsync();
                response = new ApiResponse(StatusCodes.Status200OK, "Discount deleted successfully", DiscountExisting);
                return Ok(response);
            }
            catch (Exception ex)
            {
                response = new ApiResponse(StatusCodes.Status500InternalServerError, "Internal server error", null);
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }
    }
}
