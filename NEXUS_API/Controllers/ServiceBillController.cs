using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NEXUS_API.Data;
using NEXUS_API.Helpers;
using NEXUS_API.Models;
using System;

namespace NEXUS_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ServiceBillController : ControllerBase
    {
        private readonly DatabaseContext _dbContext;

        public ServiceBillController(DatabaseContext databaseContext)
        {
            _dbContext = databaseContext;
        }

        [HttpGet]
        public async Task<ActionResult> GetServiceBills()
        {
            var data = await _dbContext.ServiceBills.ToListAsync();
            var response = new ApiResponse(StatusCodes.Status200OK, "Get all successfully", data);
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetServiceBill(int id)
        {
            object response = null;
            var serviceBill = await _dbContext.ServiceBills.FindAsync(id);
            if (serviceBill == null)
            {
                response = new ApiResponse(StatusCodes.Status404NotFound, "Not found", null);
                return Ok(response);
            }
            response = new ApiResponse(StatusCodes.Status200OK, "Get successfully", serviceBill);
            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult> CreateServiceBill(ServiceBill serviceBill)
        {
            object response = null;
            try
            {
                if (ModelState.IsValid)
                {
                    var newOrder = await _dbContext.ServiceBills.AddAsync(serviceBill);
                    await _dbContext.SaveChangesAsync();
                    response = new ApiResponse(StatusCodes.Status201Created, "Create successfully", newOrder);
                    return Created("success", response);
                }
                response = new ApiResponse(StatusCodes.Status400BadRequest, "Bad Request", null);
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                response = new ApiResponse(StatusCodes.Status500InternalServerError, "Error Server", null);
                return new ObjectResult(response);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateServiceBill(int id, ServiceBill serviceBill)
        {
            object response = null;
            if (id != serviceBill.BillId)
            {
                response = new ApiResponse(StatusCodes.Status400BadRequest, "Bad Request", null);
                return BadRequest(response);
            }
            _dbContext.Entry(serviceBill).State = EntityState.Modified;
            try
            {
                await _dbContext.SaveChangesAsync();
                response = new ApiResponse(StatusCodes.Status200OK, "Update successfully", null);
                return Ok(response);
            }
            catch (Exception ex)
            {
                response = new ApiResponse(StatusCodes.Status500InternalServerError, "Error Server", null);
                return new ObjectResult(response);
            }
        }
    }
}
