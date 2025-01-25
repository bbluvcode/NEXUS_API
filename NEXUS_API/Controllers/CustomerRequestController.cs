using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NEXUS_API.Data;
using NEXUS_API.DTOs;
using NEXUS_API.Helpers;
using NEXUS_API.Models;
using NEXUS_API.Service;
using System.Numerics;

namespace NEXUS_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerRequestController : ControllerBase
    {
        private readonly DatabaseContext _dbContext;
        public CustomerRequestController(DatabaseContext databaseContext)
        {
            _dbContext = databaseContext;
        }
        [HttpPost("create-request")]
        public async Task<IActionResult> CreateRequest([FromBody] CustomerRequestDTO customerRequestDto)
        {
            object response = null;
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }             
            try
            {
                var existingCustomer = await _dbContext.Customers.FirstOrDefaultAsync(c => c.Email == customerRequestDto.Email);
                Customer customer;
                if (existingCustomer == null)
                {
                    customer = new Customer
                    {
                        FullName = customerRequestDto.FullName,
                        Gender = customerRequestDto.Gender,
                        DateOfBirth = customerRequestDto.DateOfBirth,
                        Address = customerRequestDto.Address,
                        Email = customerRequestDto.Email,
                        PhoneNumber = customerRequestDto.PhoneNumber
                    };
                    _dbContext.Customers.Add(customer);
                    await _dbContext.SaveChangesAsync();
                }
                else
                {
                    customer = existingCustomer;
                }

                var customerRequest = new CustomerRequest
                {
                    RequestTitle = customerRequestDto.RequestTitle,
                    ServiceRequest = customerRequestDto.ServiceRequest,
                    EquipmentRequest = customerRequestDto.EquipmentRequest,
                    DateCreate = DateTime.UtcNow,
                    IsResponse = false,
                    CustomerId = customer.CustomerId,
                    RegionId = customerRequestDto.RegionId,
                };
                _dbContext.CustomerRequests.Add(customerRequest);
                await _dbContext.SaveChangesAsync();

                //create OrderId
                string orderPrefix = (customerRequestDto.ServiceRequest + " " + customerRequestDto.RequestTitle).ToLower() switch
                {
                    var s when s.Contains("dialup", StringComparison.OrdinalIgnoreCase) => "D",
                    var s when s.Contains("telephone", StringComparison.OrdinalIgnoreCase) => "T",
                    var s when s.Contains("broadband", StringComparison.OrdinalIgnoreCase) => "B",
                    _ => throw new Exception("Invalid connection type in RequestTitle or ServiceRequest")
                };
                var lastOrder = await _dbContext.ServiceOrders
                    .Where(so => so.OrderId.StartsWith(orderPrefix))
                    .OrderByDescending(so => so.OrderId)
                    .FirstOrDefaultAsync();

                int nextSerialNumber = lastOrder == null
                    ? 1
                    : int.Parse(lastOrder.OrderId.Substring(1)) + 1;

                string orderId = orderPrefix + nextSerialNumber.ToString("D10");

                decimal depositAmount = orderPrefix switch
                {
                    "D" => 325m, // Dial-Up Connection Deposit
                    "B" => 500m, // Broadband Connection Deposit
                    "T" => 250m, // Telephone Connection Deposit
                    _ => throw new Exception("Invalid connection type")
                };

                //Create order
                var serviceOrder = new ServiceOrder
                {
                    OrderId = orderId,
                    DateCreate = DateTime.UtcNow,
                    RequestId = customerRequest.RequestId,
                    SurveyStatus = "not yet",
                    Deposit = depositAmount,
                };
                _dbContext.ServiceOrders.Add(serviceOrder);
                await _dbContext.SaveChangesAsync();

                response = new ApiResponse(StatusCodes.Status201Created, "Request and ServiceOrder created successfully", serviceOrder);
                return Created("success", response);
            }
            catch (Exception ex)
            {
                response = new ApiResponse(StatusCodes.Status500InternalServerError, "Internal Server Error", null);
                return StatusCode(500, response);
            }
        }
        [HttpGet("get-requests-by-region/{regionId}")]
        public async Task<ActionResult<IEnumerable<RequestShort>>> GetRequestsByRegion(int regionId)
        {
            object response = null;
            try
            {
                var customerRequests = await _dbContext.CustomerRequests.Where(cr => cr.RegionId == regionId)
                .Select(cr => new RequestShort
                {
                    RequestId = cr.RequestId,
                    RequestTitle = cr.RequestTitle,
                    ServiceRequest = cr.ServiceRequest,
                    RegionId = cr.RegionId,
                    DateCreate = cr.DateCreate,
                    IsResponse = cr.IsResponse
                }).ToListAsync();
                if (customerRequests == null || customerRequests.Count == 0)
                {
                    response = new ApiResponse(StatusCodes.Status400BadRequest, "No customer requests found for the specified region", null);
                    return NotFound(response);
                }
                response = new ApiResponse(StatusCodes.Status200OK, "GetRequestsByRegion successfully", customerRequests);
                return Ok(response);
            }
            catch (Exception ex)
            {
                response = new ApiResponse(StatusCodes.Status500InternalServerError, "Internal Server Error", null);
                return StatusCode(500, response);
            }
        }
    }
}
