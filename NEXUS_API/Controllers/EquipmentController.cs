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
    public class EquipmentController : ControllerBase
    {
        private readonly DatabaseContext _dbContext;
        public EquipmentController(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllEquipments()
        {
            try
            {
                var equipments = await _dbContext.Equipments
                    .Include(e=>e.Discount)
                    .Include(e=>e.EquipmentType)
                    .ToListAsync();
                var response = new ApiResponse(StatusCodes.Status200OK, "Get all successfully", equipments);
                return Ok(response);
            }
            catch (Exception ex)
            {
                var response = new ApiResponse(StatusCodes.Status500InternalServerError, "Internal Server Error", null);
                return StatusCode(500, response);
            }
        }
        [HttpGet("GetByType/{type}")]
        public async Task<IActionResult> GetEquipmentsByType(int type)
        {
            try
            {
                var equipments = await _dbContext.Equipments
                    .Where(e=>e.EquipmentTypeId == type)
                    .ToListAsync();
                var response = new ApiResponse(StatusCodes.Status200OK, "Get Equipments By Type successfully", equipments);
                return Ok(response);
            }
            catch (Exception ex)
            {
                var response = new ApiResponse(StatusCodes.Status500InternalServerError, "Internal Server Error", null);
                return StatusCode(500, response);
            }
        }
        [HttpPost]
        public async Task<IActionResult> AddEquipment([FromBody] Equipment Equipment)
        {
            object response = null;
            try
            {
                await _dbContext.Equipments.AddAsync(Equipment);
                await _dbContext.SaveChangesAsync();
                response = new ApiResponse(StatusCodes.Status201Created, "Equipment created successfully", Equipment);
                return Created("success", response);
            }
            catch (Exception ex)
            {
                response = new ApiResponse(StatusCodes.Status500InternalServerError, "Internal Server Error", null);
                return new ObjectResult(response);
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEquipment(int id, [FromBody] Equipment Equipment)
        {
            object response = null;
            try
            {
                var equipmentExisting = await _dbContext.Equipments.FindAsync(id);
                if (equipmentExisting == null)
                {
                    response = new ApiResponse(StatusCodes.Status404NotFound, "Equipment not found", null);
                    return NotFound(response);
                }
                equipmentExisting.EquipmentId = id;
                equipmentExisting.StockQuantity = Equipment.StockQuantity;
                equipmentExisting.Price = Equipment.Price;
                equipmentExisting.Status = Equipment.Status;
                equipmentExisting.DiscountId = Equipment.DiscountId;
                await _dbContext.SaveChangesAsync();
                response = new ApiResponse(StatusCodes.Status200OK, "Equipment updated successfully", equipmentExisting);
                return Ok(response);
            }
            catch (Exception ex)
            {
                response = new ApiResponse(StatusCodes.Status500InternalServerError, "Internal server error", null);
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEquipment(int id)
        {
            object response = null;
            try
            {
                var equipmentExisting = await _dbContext.Equipments.FindAsync(id);
                if (equipmentExisting == null)
                {
                    response = new ApiResponse(StatusCodes.Status404NotFound, "Equipment not found", null);
                    return NotFound(response);
                }
                _dbContext.Equipments.Remove(equipmentExisting);
                await _dbContext.SaveChangesAsync();
                response = new ApiResponse(StatusCodes.Status200OK, "Equipment deleted successfully", equipmentExisting);
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
