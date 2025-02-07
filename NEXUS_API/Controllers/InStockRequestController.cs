using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using NEXUS_API.Models;
using NEXUS_API.Data;
using NEXUS_API.Helpers;

namespace NEXUS_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InStockRequestController : ControllerBase
    {
        private readonly DatabaseContext _dbContext;

        public InStockRequestController(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET: api/InStockRequest
        [HttpGet]
        public async Task<IActionResult> GetInStockRequests()
        {
            var requests = await _dbContext.InStockRequests
                .Include(r => r.Employee)
                .Include(r => r.InStockRequestDetails)
                .Include(r => r.InStockOrders)
                .ToListAsync();

            return Ok(new ApiResponse(200, "Get in-stock requests successfully", requests));
        }

        // GET: api/InStockRequest/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetInStockRequest(int id)
        {
            var request = await _dbContext.InStockRequests
                .Include(r => r.Employee)
                .Include(r => r.InStockRequestDetails)
                .Include(r => r.InStockOrders)
                .FirstOrDefaultAsync(r => r.InStockRequestId == id);

            if (request == null)
                return NotFound(new ApiResponse(404, "In-stock request not found", null));

            return Ok(new ApiResponse(200, "Get in-stock request successfully", request));
        }

        // POST: api/InStockRequest
        [HttpPost]
        public async Task<IActionResult> CreateInStockRequest([FromBody] InStockRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse(400, "Invalid data", null));

            _dbContext.InStockRequests.Add(request);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetInStockRequest), new { id = request.InStockRequestId }, new ApiResponse(201, "Created in-stock request successfully", request));
        }

        // PUT: api/InStockRequest/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateInStockRequest(int id, [FromBody] InStockRequest requestUpdate)
        {
            var request = await _dbContext.InStockRequests.FindAsync(id);
            if (request == null)
                return NotFound(new ApiResponse(404, "In-stock request not found", null));

            request.EmployeeId = requestUpdate.EmployeeId;
            request.CreateDate = requestUpdate.CreateDate;
            request.TotalNumber = requestUpdate.TotalNumber;

            await _dbContext.SaveChangesAsync();
            return Ok(new ApiResponse(200, "Updated in-stock request successfully", request));
        }

        // DELETE: api/InStockRequest/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInStockRequest(int id)
        {
            var request = await _dbContext.InStockRequests.FindAsync(id);
            if (request == null)
                return NotFound(new ApiResponse(404, "In-stock request not found", null));

            _dbContext.InStockRequests.Remove(request);
            await _dbContext.SaveChangesAsync();

            return Ok(new ApiResponse(200, "Deleted in-stock request successfully", request));
        }

        // ----------------------------------------------
        // API FOR IN-STOCK REQUEST DETAILS
        // ----------------------------------------------

        // GET: api/InStockRequest/{requestId}/details
        [HttpGet("{requestId}/details")]
        public async Task<IActionResult> GetInStockRequestDetails(int requestId)
        {
            var details = await _dbContext.InStockRequestDetails
                .Include(d => d.Equipment)
                .Where(d => d.InStockRequestId == requestId)
                .ToListAsync();

            return Ok(new ApiResponse(200, "Get in-stock request details successfully", details));
        }

        // POST: api/InStockRequest/{requestId}/details
        [HttpPost("{requestId}/details")]
        public async Task<IActionResult> CreateInStockRequestDetail(int requestId, [FromBody] InStockRequestDetail detail)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse(400, "Invalid data", null));

            detail.InStockRequestId = requestId;
            _dbContext.InStockRequestDetails.Add(detail);
            await _dbContext.SaveChangesAsync();

            return Created("success", new ApiResponse(201, "Created in-stock request detail successfully", detail));
        }

        // PUT: api/InStockRequest/details/{id}
        [HttpPut("details/{id}")]
        public async Task<IActionResult> UpdateInStockRequestDetail(int id, [FromBody] InStockRequestDetail detailUpdate)
        {
            var detail = await _dbContext.InStockRequestDetails.FindAsync(id);
            if (detail == null)
                return NotFound(new ApiResponse(404, "In-stock request detail not found", null));

            detail.EquipmentId = detailUpdate.EquipmentId;
            detail.Quantity = detailUpdate.Quantity;

            await _dbContext.SaveChangesAsync();
            return Ok(new ApiResponse(200, "Updated in-stock request detail successfully", detail));
        }

        // DELETE: api/InStockRequest/details/{id}
        [HttpDelete("details/{id}")]
        public async Task<IActionResult> DeleteInStockRequestDetail(int id)
        {
            var detail = await _dbContext.InStockRequestDetails.FindAsync(id);
            if (detail == null)
                return NotFound(new ApiResponse(404, "In-stock request detail not found", null));

            _dbContext.InStockRequestDetails.Remove(detail);
            await _dbContext.SaveChangesAsync();

            return Ok(new ApiResponse(200, "Deleted in-stock request detail successfully", detail));
        }
    }
}
