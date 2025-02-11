using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NEXUS_API.Data;
using NEXUS_API.DTOs;
using NEXUS_API.Helpers;
using NEXUS_API.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

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
            var equipments = await _dbContext.Equipments
                .AsNoTracking()  
                .Include(e => e.Discount)
                .Include(e => e.EquipmentType)
                .Include(e => e.Vendor)
                .Include(e => e.Stock)
                .ToListAsync();

            return Ok(new ApiResponse(StatusCodes.Status200OK, "Get all successfully", equipments));
        }

        [HttpGet("GetByType/{type}")]
        public async Task<IActionResult> GetEquipmentsByType(int type)
        {
            var equipments = await _dbContext.Equipments
                .AsNoTracking()
                .Where(e => e.EquipmentTypeId == type)
                .ToListAsync();

            return Ok(new ApiResponse(StatusCodes.Status200OK, "Get Equipments By Type successfully", equipments));
        }

        [HttpPost]
        public async Task<IActionResult> AddEquipment([FromForm] EquipmentDTO equipmentDto, IFormFile imageFile)
        {
            try
            {
                var equipment = new Equipment
                {
                    EquipmentName = equipmentDto.EquipmentName,
                    Price = equipmentDto.Price,
                    StockQuantity = equipmentDto.StockQuantity,
                    Description = equipmentDto.Description,
                    Status = equipmentDto.Status,
                    DiscountId = equipmentDto.DiscountId,
                    EquipmentTypeId = equipmentDto.EquipmentTypeId,
                    VendorId = equipmentDto.VendorId,
                    StockId = equipmentDto.StockId,
                    Image = imageFile != null ? await UploadFile.SaveImage("imageEquipment", imageFile) : null
                };

                await _dbContext.Equipments.AddAsync(equipment);
                await _dbContext.SaveChangesAsync();

                return Created("success", new ApiResponse(StatusCodes.Status201Created, "Equipment created successfully", equipment));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, $"Error: {ex.Message}", null));
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEquipment(int id, [FromForm] EquipmentDTO equipmentDto, IFormFile imageFile)
        {
            try
            {
                var existingEquipment = await _dbContext.Equipments.FindAsync(id);
                if (existingEquipment == null)
                {
                    return NotFound(new ApiResponse(StatusCodes.Status404NotFound, "Equipment not found", null));
                }

                existingEquipment.EquipmentName = equipmentDto.EquipmentName;
                existingEquipment.Price = equipmentDto.Price;
                existingEquipment.StockQuantity = equipmentDto.StockQuantity;
                existingEquipment.Description = equipmentDto.Description;
                existingEquipment.Status = equipmentDto.Status;
                existingEquipment.DiscountId = equipmentDto.DiscountId;
                existingEquipment.EquipmentTypeId = equipmentDto.EquipmentTypeId;
                existingEquipment.VendorId = equipmentDto.VendorId;
                existingEquipment.StockId = equipmentDto.StockId;

                // Cập nhật ảnh nếu có file mới
                if (imageFile != null)
                {
                    if (!string.IsNullOrEmpty(existingEquipment.Image))
                    {
                        UploadFile.DeleteImage(existingEquipment.Image);
                    }
                    existingEquipment.Image = await UploadFile.SaveImage("imageEquipment", imageFile);
                }

                await _dbContext.SaveChangesAsync();
                return Ok(new ApiResponse(StatusCodes.Status200OK, "Equipment updated successfully", existingEquipment));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, $"Error: {ex.Message}", null));
            }
        }
    }
}
