using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NEXUS_API.Data;
using NEXUS_API.Helpers;
using NEXUS_API.Models;
using System;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace NEXUS_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ServiceOrderController : ControllerBase
    {
        private readonly DatabaseContext _dbContext;

        public ServiceOrderController(DatabaseContext databaseContext)
        {
            _dbContext = databaseContext;
        }

        [HttpGet]
        public async Task<ActionResult> GetServiceOrders()
        {
            var data =  await _dbContext.ServiceOrders.ToListAsync();
            var response = new ApiResponse(StatusCodes.Status200OK, "Get all successfully", data);
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetServiceOrder(int id)
        {
            object response = null;
            var serviceOrder = await _dbContext.ServiceOrders.FindAsync(id);
            if (serviceOrder == null)
            {
                response = new ApiResponse(StatusCodes.Status404NotFound, "Not found", null);
                return Ok(response);
            }
            response = new ApiResponse(StatusCodes.Status200OK, "Get successfully", serviceOrder);
            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult> CreateServiceOrder(ServiceOrder serviceOrder)
        {
            object response = null;
            try
            {
                if (ModelState.IsValid)
                {
                    var newOrder = await _dbContext.ServiceOrders.AddAsync(serviceOrder);
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
        public async Task<IActionResult> UpdateServiceOrder(int id, ServiceOrder serviceOrder)
        {
            object response = null;
            if (id != serviceOrder.OrderId)
            {
                response = new ApiResponse(StatusCodes.Status400BadRequest, "Bad Request", null);
                return BadRequest(response);
            }
            _dbContext.Entry(serviceOrder).State = EntityState.Modified;
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
        [HttpGet("get-by-customer/{customerId}")]
        public async Task<ActionResult> GetServiceOrdersByCustomer(int customerId)
        {
            object response = null;
            var data = await _dbContext.ServiceOrders.Where(o => o.CustomerId == customerId).ToListAsync();
            if (data == null || data.Count == 0)
            {
                response = new ApiResponse(StatusCodes.Status404NotFound, "No orders found for this customer", null);
                return NotFound(response);
            }
            response = new ApiResponse(StatusCodes.Status200OK, "Get orders successfully", data);
            return Ok(response);
        }
    }
}
