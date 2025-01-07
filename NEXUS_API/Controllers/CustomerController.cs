using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Threading.Tasks;
using NEXUS_API.Models;
using NEXUS_API.Data;
using NEXUS_API.Helpers;
using NEXUS_API.DTOs;

namespace NEXUS_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly DatabaseContext _dbContext;

        public CustomerController(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetCustomers()
        {
            var customers = await _dbContext.Customers.ToListAsync();
            var response = new ApiResponse(StatusCodes.Status200OK, "get customers successfully", customers);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCustomer([FromBody] Customer customer)
        {
            object response = null;
            try
            {
                if (ModelState.IsValid)
                {
                    await _dbContext.Customers.AddAsync(customer);
                    await _dbContext.SaveChangesAsync();
                    response = new ApiResponse(StatusCodes.Status201Created, "create customer successfully", customer);
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
        public async Task<IActionResult> UpdateCustomer(int id, [FromBody] Customer customerUpdate)
        {
            object response = null;
            try
            {
                var customer = await _dbContext.Customers.FirstOrDefaultAsync(c => c.CustomerId == id);
                if (customer == null)
                {
                    response = new ApiResponse(StatusCodes.Status404NotFound, "customer not found", null);
                    return NotFound(response);
                }

                if (!ModelState.IsValid)
                {
                    response = new ApiResponse(StatusCodes.Status400BadRequest, "bad request", null);
                    return BadRequest(response);
                }

                customer.FullName = customerUpdate.FullName;
                customer.Gender = customerUpdate.Gender;
                customer.DateOfBirth = customerUpdate.DateOfBirth;
                customer.Address = customerUpdate.Address;
                customer.Email = customerUpdate.Email;
                customer.PhoneNumber = customerUpdate.PhoneNumber;
                customer.IdentificationNo = customerUpdate.IdentificationNo;
                customer.Image = customerUpdate.Image;

                await _dbContext.SaveChangesAsync();
                response = new ApiResponse(StatusCodes.Status200OK, "update customer successfully", customer);
                return Ok(response);
            }
            catch (Exception)
            {
                response = new ApiResponse(StatusCodes.Status500InternalServerError, "server error", null);
                return StatusCode(500, response);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            object response = null;
            try
            {
                var customer = await _dbContext.Customers.FirstOrDefaultAsync(c => c.CustomerId == id);
                if (customer == null)
                {
                    response = new ApiResponse(StatusCodes.Status404NotFound, "customer not found", null);
                    return NotFound(response);
                }

                _dbContext.Customers.Remove(customer);
                await _dbContext.SaveChangesAsync();
                response = new ApiResponse(StatusCodes.Status200OK, "delete customer successfully", customer);
                return Ok(response);
            }
            catch (Exception)
            {
                response = new ApiResponse(StatusCodes.Status500InternalServerError, "server error", null);
                return StatusCode(500, response);
            }
        }

        //=====================CUSMER REQUEST======================
        [HttpGet("all-customer-request")]
        public async Task<IActionResult> GetAllCustomerRequest()
        {
            var list = await _dbContext.CustomerRequests.Include(x=>x.Customer).ToListAsync();
            var customerRequestList = list.Select(x=>new CustomerRequestDTO
            {
                CustomerId = x.CustomerId,
                RequestId = x.RequestId,
                RequestTitle = x.RequestTitle,
                ServiceRequest =x.ServiceRequest,
                EquipmentRequest =x.EquipmentRequest,
                IsResponse = x.IsResponse,
                FullName = x.Customer.FullName,
                Gender = x.Customer.Gender,
                DateOfBirth = x.Customer.DateOfBirth,
                Address = x.Customer.Address,
                Email = x.Customer.Email,
                PhoneNumber = x.Customer.PhoneNumber,
            }).ToList();
            var response = new ApiResponse(StatusCodes.Status200OK, "Get list of customer request successfully", customerRequestList);
            return Ok(response);
        }

        [HttpGet("customer-request-by-customerID/{cusID}")]
        public async Task<IActionResult> GetCustomerRequestByCusID(int cusID)
        {
            var list = await _dbContext.CustomerRequests
                .Where(x=>x.CustomerId == cusID)
                .Include(x => x.Customer)
                .ToListAsync();
            var customerRequestList = list.Select(x => new CustomerRequestDTO
            {
                CustomerId = x.CustomerId,
                RequestId = x.RequestId,
                RequestTitle = x.RequestTitle,
                ServiceRequest = x.ServiceRequest,
                EquipmentRequest = x.EquipmentRequest,
                IsResponse = x.IsResponse,
                FullName = x.Customer.FullName,
                Gender = x.Customer.Gender,
                DateOfBirth = x.Customer.DateOfBirth,
                Address = x.Customer.Address,
                Email = x.Customer.Email,
                PhoneNumber = x.Customer.PhoneNumber,
            }).ToList();
            var response = new ApiResponse(StatusCodes.Status200OK, "Get Requests of CustomerID: successfully", customerRequestList);
            return Ok(response);
        }

        [HttpPost("create-customer-request")]
        public async Task<IActionResult> CreateCustomerRequest([FromForm] CustomerRequest cusReq)
        {
            object response = null;
            try
            {
                if (ModelState.IsValid)
                {
                    await _dbContext.CustomerRequests.AddAsync(cusReq);
                    await _dbContext.SaveChangesAsync();
                    response = new ApiResponse(StatusCodes.Status200OK, "Create customer request successfully", cusReq);
                    return Created("success", response);
                }
                response = new ApiResponse(StatusCodes.Status400BadRequest, "Create customer request fail", null);
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                response = new ApiResponse(StatusCodes.Status500InternalServerError, "Server error, loi sql da ton tai fk", null);
                return StatusCode(500, response);

            }
        }

        [HttpPut("update-customer-request")]
        public async Task<IActionResult> UpdateCustomerRequest([FromForm] CustomerRequest cusReq)
        {
            object response = null;
            try
            {
                if (ModelState.IsValid)
                {
                    var existingReq = await _dbContext.CustomerRequests.FirstOrDefaultAsync(x => x.RequestId == cusReq.RequestId);
                    if(existingReq != null)
                    {
                        _dbContext.Entry(existingReq).CurrentValues.SetValues(cusReq);
                        await _dbContext.SaveChangesAsync();

                        response = new ApiResponse(StatusCodes.Status200OK, "Update customer request successfully!", cusReq);
                        return Ok(response);
                    }
                    response = new ApiResponse(StatusCodes.Status404NotFound, "customer request not found", null);
                    return NotFound(response);
            }
                response = new ApiResponse(StatusCodes.Status400BadRequest, "Invalid data", null);
            return BadRequest(response);
            }
            catch (Exception ex)
            {
                response = new ApiResponse(StatusCodes.Status500InternalServerError, "server error: " + ex.Message, null);
                return StatusCode(500, response);
            }
        }
        [HttpDelete("delete-customer-request/{cusReqID}")]
        public async Task<IActionResult> DeleteCustomerRequest(int cusReqID)
        {
            object response = null;
            try
            {
                var existingCusReq = await _dbContext.CustomerRequests.FirstOrDefaultAsync(x => x.RequestId == cusReqID);
                if(existingCusReq != null)
                {
                    _dbContext.CustomerRequests.Remove(existingCusReq);
                    await _dbContext.SaveChangesAsync();
                    response = new ApiResponse(StatusCodes.Status200OK, "DELETE customer request successfully", existingCusReq);
                    return Ok(response);
                }
                response = new ApiResponse(StatusCodes.Status404NotFound, "Cart item not found", null);
                return NotFound(response);
            }
            catch (Exception ex)
            {
                response = new ApiResponse(StatusCodes.Status500InternalServerError, "Server error: " + ex.Message, null);
                return StatusCode(500, response);
            }
        }

    }
}
