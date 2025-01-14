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
        public async Task<IActionResult> CreateCustomer([FromForm] Customer customer)
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
        public async Task<IActionResult> UpdateCustomer(int id, [FromForm] Customer customerUpdate)
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
            var list = await _dbContext.CustomerRequests.Include(x => x.Customer).ToListAsync();
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
                DateCreate = x.DateCreate,
                DateResolve = x.DateResolve,
            }).ToList();
            var response = new ApiResponse(StatusCodes.Status200OK, "Get list of customer request successfully", customerRequestList);
            return Ok(response);
        }

        [HttpGet("customer-request-by-customerID/{cusID}")]
        public async Task<IActionResult> GetCustomerRequestByCusID(int cusID)
        {
            var list = await _dbContext.CustomerRequests
                .Where(x => x.CustomerId == cusID)
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
                DateCreate = x.DateCreate,
                DateResolve = x.DateResolve,
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
                    if (existingReq != null)
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
                if (existingCusReq != null)
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

        [HttpPut("change-status-customer-request/{reqID}")]
        public async Task<IActionResult> ChangeStatusCustomerRequest(int reqID)
        {
            object response = null;
            try
            {
                var existingReq = await _dbContext.CustomerRequests.FirstOrDefaultAsync(x => x.RequestId == reqID);
                if (existingReq != null)
                {
                    existingReq.IsResponse = !existingReq.IsResponse;
                    if (existingReq.IsResponse)
                    {
                        existingReq.DateResolve = DateTime.Now; // Fixed the DateTime.Now() to DateTime.Now
                    }
                    else
                    {
                        existingReq.DateResolve = null;
                    }
                    await _dbContext.SaveChangesAsync();
                    response = new ApiResponse(StatusCodes.Status200OK, "Change status customer request successfully!", existingReq);
                    return Ok(response);
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


        //=====================END - CUSMER REQUEST======================
        //===============================================================
        //=====================SUPPORT REQUEST===========================
        [HttpGet("support-requests")]
        public async Task<IActionResult> GetSupportRequests()
        {
            var supportRequests = await _dbContext.SupportRequests
                .Include(x => x.Customer)
                .Include(x => x.Employee)
                .ToListAsync();

            var supportRequestDTOs = supportRequests.Select(sr => new SupportRequestDTO
            {
                SupportRequestId = sr.SupportRequestId,
                DateRequest = sr.DateRequest,
                CustomerId = sr.CustomerId,
                Title = sr.Title,
                DetailContent = sr.DetailContent,
                DateResolved = sr.DateResolved,
                IsResolved = sr.IsResolved,
                FullName = sr.Customer.FullName,
                Gender = sr.Customer.Gender,
                DateOfBirth = sr.Customer.DateOfBirth,
                Address = sr.Customer.Address,
                Email = sr.Customer.Email,
                PhoneNumber = sr.Customer.PhoneNumber
            }).ToList();

            var response = new ApiResponse(StatusCodes.Status200OK, "Get support requests successfully", supportRequestDTOs);
            return Ok(response);
        }


        [HttpPost("create-support-request")]
        public async Task<IActionResult> CreateSupportRequest([FromForm] SupportRequest supportRequest)
        {
            object response = null;
            try
            {
                if (ModelState.IsValid)
                {
                    supportRequest.IsResolved = false;
                    supportRequest.DateRequest = DateTime.Now;
                    supportRequest.DateResolved = null;

                    await _dbContext.SupportRequests.AddAsync(supportRequest);
                    await _dbContext.SaveChangesAsync();

                    response = new ApiResponse(StatusCodes.Status201Created, "Create support request successfully", supportRequest);
                    return Created("success", response);
                }

                response = new ApiResponse(StatusCodes.Status400BadRequest, "Invalid support request data", null);
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                response = new ApiResponse(StatusCodes.Status500InternalServerError, $"Server error: {ex.Message}", null);
                return StatusCode(500, response);
            }
        }

        [HttpPut("resolve-support-request/{id}")]
        public async Task<IActionResult> ResolveSupportRequest(int id, [FromForm] int empIdResolver)
        {
            object response = null;
            try
            {
                var supportRequest = await _dbContext.SupportRequests.FirstOrDefaultAsync(sr => sr.SupportRequestId == id);
                if (supportRequest == null)
                {
                    response = new ApiResponse(StatusCodes.Status404NotFound, "Support request not found", null);
                    return NotFound(response);
                }

                supportRequest.IsResolved = true;
                supportRequest.DateResolved = DateTime.Now;
                supportRequest.EmpIdResolver = empIdResolver;

                await _dbContext.SaveChangesAsync();

                response = new ApiResponse(StatusCodes.Status200OK, "Resolve support request successfully", supportRequest);
                return Ok(response);
            }
            catch (Exception ex)
            {
                response = new ApiResponse(StatusCodes.Status500InternalServerError, $"Server error: {ex.Message}", null);
                return StatusCode(500, response);
            }
        }
        //=====================END - SUPPORT REQUEST======================
        //================================================================
        //=====================FEED BACKS=================================
        [HttpGet("feedbacks")]
        public async Task<IActionResult> GetFeedbacks()
        {
            var feedbacks = await _dbContext.FeedBacks
                .Include(x => x.Customer)
                .ToListAsync();

            var feedbackDTOs = feedbacks.Select(fb => new FeedbackDTO
            {
                FeedBackId = fb.FeedBackId,
                Date = fb.Date,
                Title = fb.Title,
                FeedBackContent = fb.FeedBackContent,
                Status = fb.Status,
                CustomerId = fb.Customer.CustomerId,
                FullName = fb.Customer.FullName,
                Gender = fb.Customer.Gender,
                DateOfBirth = fb.Customer.DateOfBirth,
                Address = fb.Customer.Address,
                Email = fb.Customer.Email,
                PhoneNumber = fb.Customer.PhoneNumber
            }).ToList();

            var response = new ApiResponse(StatusCodes.Status200OK, "Get feedbacks successfully", feedbackDTOs);
            return Ok(response);
        }

        [HttpPost("create-feedback")]
        public async Task<IActionResult> CreateFeedback([FromForm] FeedBack feedback)
        {
            object response = null;
            try
            {
                if (ModelState.IsValid)
                {
                    await _dbContext.FeedBacks.AddAsync(feedback);
                    await _dbContext.SaveChangesAsync();

                    response = new ApiResponse(StatusCodes.Status201Created, "Create feedback successfully", feedback);
                    return Created("success", response);
                }

                response = new ApiResponse(StatusCodes.Status400BadRequest, "Invalid feedback data", null);
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                response = new ApiResponse(StatusCodes.Status500InternalServerError, $"Server error: {ex.Message}", null);
                return StatusCode(500, response);
            }
        }

        [HttpPut("update-feedback-status/{id}")]
        public async Task<IActionResult> UpdateFeedbackStatus(int id, [FromForm] bool status)
        {
            object response = null;
            try
            {
                var feedback = await _dbContext.FeedBacks.FirstOrDefaultAsync(fb => fb.FeedBackId == id);
                if (feedback == null)
                {
                    response = new ApiResponse(StatusCodes.Status404NotFound, "Feedback not found", null);
                    return NotFound(response);
                }

                feedback.Status = status;

                await _dbContext.SaveChangesAsync();

                response = new ApiResponse(StatusCodes.Status200OK, "Update feedback status successfully", feedback);
                return Ok(response);
            }
            catch (Exception ex)
            {
                response = new ApiResponse(StatusCodes.Status500InternalServerError, $"Server error: {ex.Message}", null);
                return StatusCode(500, response);
            }
        }

        [HttpPut("change-status-feedback-status/{fbID}")]
        public async Task<IActionResult> ChangeStatusFeedback(int fbID)
        {
            object response = null;
            try
            {
                var existingfb = await _dbContext.FeedBacks.FirstOrDefaultAsync(x => x.FeedBackId == fbID);
                if (existingfb != null)
                {
                    existingfb.Status = !existingfb.Status;
                    await _dbContext.SaveChangesAsync();
                    response = new ApiResponse(StatusCodes.Status200OK, "Change status feedback successfully!", existingfb);
                    return Ok(response);
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
    }
}
