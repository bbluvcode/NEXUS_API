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
    public class EquipmentTypeController : ControllerBase
    {
        private readonly DatabaseContext _dbContext;
        public EquipmentTypeController(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllTypes()
        {
            try
            {
                var types = await _dbContext.EquipmentTypes.ToListAsync();
                var response = new ApiResponse(StatusCodes.Status200OK, "Get all successfully", types);
                return Ok(response);
            }
            catch (Exception ex)
            {
                var response = new ApiResponse(StatusCodes.Status500InternalServerError, "Internal Server Error", null);
                return StatusCode(500, response);
            }
        }
        [HttpPost]
        public async Task<IActionResult> AddType([FromBody] EquipmentType equipmentType)
        {
            object response = null;
            try
            {
                await _dbContext.EquipmentTypes.AddAsync(equipmentType);
                await _dbContext.SaveChangesAsync();
                response = new ApiResponse(StatusCodes.Status201Created, "EquipmentType created successfully", equipmentType);
                return Created("success", response);
            }
            catch (Exception ex)
            {
                response = new ApiResponse(StatusCodes.Status500InternalServerError, "Internal Server Error", null);
                return new ObjectResult(response);
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateType(int id,[FromBody] EquipmentType equipmentType)
        {
            object response = null;
            try
            {
                var typeExisting = await _dbContext.EquipmentTypes.FirstOrDefaultAsync(ep => ep.EquipmentTypeId == id);
                if (typeExisting == null)
                {
                    response = new ApiResponse(StatusCodes.Status404NotFound, "EquipmentType not found", null);
                    return NotFound(response);
                }
                typeExisting.EquipmentTypeId = id;
                typeExisting.TypeName = equipmentType.TypeName;
                typeExisting.Provider = equipmentType.Provider;
                await _dbContext.SaveChangesAsync();
                response = new ApiResponse(StatusCodes.Status200OK, "EquipmentType updated successfully", typeExisting);
                return Ok(response);
            }
            catch (Exception ex)
            {
                response = new ApiResponse(StatusCodes.Status500InternalServerError, "Internal server error", null);
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteType(int id)
        {
            object response = null;
            try
            {
                var typeExisting = await _dbContext.EquipmentTypes.FindAsync(id);
                if (typeExisting == null)
                {
                    response = new ApiResponse(StatusCodes.Status404NotFound, "EquipmentType not found", null);
                    return NotFound(response);
                }
                _dbContext.EquipmentTypes.Remove(typeExisting);
                await _dbContext.SaveChangesAsync();
                response = new ApiResponse(StatusCodes.Status200OK, "EquipmentType deleted successfully", typeExisting);
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
