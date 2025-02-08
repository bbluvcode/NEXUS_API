using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Threading.Tasks;
using NEXUS_API.Models;
using NEXUS_API.Data;
using NEXUS_API.Helpers;
using NEXUS_API.DTOs;
using NEXUS_API.Service;
using Azure;
using Microsoft.Extensions.Configuration;

namespace NEXUS_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly DatabaseContext _dbContext;
        private readonly string subFolder = "customerImages";
        private readonly PayPalService _payPalService;
        private readonly IConfiguration _configuration;
        private readonly EmailService _emailService;
        public CustomerController(DatabaseContext dbContext, PayPalService payPalService, IConfiguration configuration, EmailService emailService)
        {
            _dbContext = dbContext;
            _payPalService = payPalService;
            _configuration = configuration;
            _emailService = emailService;
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


        [HttpGet("customer-info/{email}")]
        public async Task<IActionResult> GetCustomerInfo(string email)
        {
            var customer = await _dbContext.Customers                
                .FirstOrDefaultAsync(c => c.Email == email);

            if (customer == null)
            {
                return NotFound(new ApiResponse(StatusCodes.Status404NotFound, "Customer not found", null));
            }

            var response = new ApiResponse(StatusCodes.Status200OK, "Customer retrieved successfully", customer);
            return Ok(response);
        }

        [HttpGet("customer-by-email/{email}")]
        public async Task<IActionResult> GetCustomerByEmail(string email)
        {
            var customer = await _dbContext.Customers
                        .Include(c => c.SupportRequests)
                        .Include(c => c.CustomerRequests)
                        .Include(c => c.Accounts)
                        .Include(c => c.FeedBacks)
                        .Include(c => c.Accounts)
                        .ThenInclude(a => a.ServiceOrders) 
                        .ThenInclude(o => o.Connections)
                .FirstOrDefaultAsync(c => c.Email == email);

            if (customer == null)
            {
                return NotFound(new ApiResponse(StatusCodes.Status404NotFound, "Customer not found", null));
            }

            var response = new ApiResponse(StatusCodes.Status200OK, "Customer retrieved successfully", customer);
            return Ok(response);
        }

        [HttpPut("update-info/{id}")]
        public async Task<IActionResult> UpdateInfomation(int id, [FromBody] CustomerUpdateInfoDTO userUpdateDTO)
        {
            object response = null;
            try
            {
                var user = await _dbContext.Customers.FirstOrDefaultAsync(c => c.CustomerId == id);
                if (user == null)
                {
                    response = new ApiResponse(StatusCodes.Status404NotFound, "Customer not found", null);
                    return NotFound(response);
                }

                user.FullName = userUpdateDTO.FullName ?? user.FullName;
                user.PhoneNumber = userUpdateDTO.PhoneNumber ?? user.PhoneNumber;
                user.Gender = userUpdateDTO.Gender ?? user.Gender;
                user.Address = userUpdateDTO.Address ?? user.Address;
                user.DateOfBirth = userUpdateDTO.DOB ?? user.DateOfBirth;

                await _dbContext.SaveChangesAsync();

                response = new ApiResponse(StatusCodes.Status200OK, "Customer updated successfully", user);
                return Ok(response);
            }
            catch (Exception ex)
            {
                response = new ApiResponse(StatusCodes.Status500InternalServerError, "Internal server error", null);
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        [HttpPut("update-image/{id}")]
        public async Task<IActionResult> UpdateImage(int id, IFormFile? file)
        {
            object response = null;
            try
            {
                var userExisting = await _dbContext.Customers.FirstOrDefaultAsync(c => c.CustomerId == id);
                if (userExisting == null)
                {
                    response = new ApiResponse(StatusCodes.Status404NotFound, "Customer not found", null);
                    return NotFound(response);
                }

                if (file == null || file.Length == 0)
                {
                    response = new ApiResponse(StatusCodes.Status400BadRequest, "No image provided", null);
                    return BadRequest(response);
                }

                var imagePath = await UploadFile.SaveImage(subFolder, file);
                userExisting.Image = imagePath;

                await _dbContext.SaveChangesAsync();

                response = new ApiResponse(StatusCodes.Status200OK, "Updated successfully", userExisting);
                return Ok(response);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating image: {ex}");
                response = new ApiResponse(StatusCodes.Status500InternalServerError, "Internal server error" + ex.Message, null);
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        [HttpPut("change-password/{id}")]
        public async Task<IActionResult> ChangePassword(int id, [FromBody] ChangePasswordDTO model)
        {
            if (model == null || string.IsNullOrWhiteSpace(model.OldPassword) || string.IsNullOrWhiteSpace(model.NewPassword))
            {
                return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Invalid request data", null));
            }

            try
            {
                var user = await _dbContext.Customers.FirstOrDefaultAsync(c => c.CustomerId == id);
                if (user == null)
                {
                    return NotFound(new ApiResponse(StatusCodes.Status404NotFound, "Customer not found", null));
                }

                if (model.OldPassword != user.Password)
                {
                    return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Current password is incorrect", null));
                }
                if (model.OldPassword == model.NewPassword)
                {
                    return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Current password and New password match", null));
                }
                user.Password = model.NewPassword;
                await _dbContext.SaveChangesAsync();

                return Ok(new ApiResponse(StatusCodes.Status200OK, "Password changed successfully", null));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse(StatusCodes.Status500InternalServerError, "Internal server error", null));
            }
        }


        //=====================CUSMER REQUEST======================
        [HttpGet("all-customer-request")]
        public async Task<IActionResult> GetAllCustomerRequest()
        {
            var list = await _dbContext.CustomerRequests.Include(x => x.Customer).Include(x=>x.Region).ToListAsync();
            var customerRequestList = list.Select(x => new CustomerRequestDTO
            {
                RequestId = x.RequestId,
                RequestTitle = x.RequestTitle,
                ServiceRequest = x.ServiceRequest,
                EquipmentRequest = x.EquipmentRequest,
                IsResponse = x.IsResponse,
                RegionId = x.RegionId,
                RegionCode = x.Region.RegionCode,
                RegionName = x.Region.RegionName,
                Deposit = x.Deposit,
                DepositStatus = x.DepositStatus,
                InstallationAddress = x.InstallationAddress,
                FullName = x.Customer.FullName,
                Gender = x.Customer.Gender,
                DateOfBirth = x.Customer.DateOfBirth,
                Address = x.Customer.Address,
                Email = x.Customer.Email,
                PhoneNumber = x.Customer.PhoneNumber,
                DateCreate = x.DateCreate,
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
                .Include(x => x.Region)
                .ToListAsync();
            var customerRequestList = list.Select(x => new CustomerRequestDTO
            {
                RequestId = x.RequestId,
                RequestTitle = x.RequestTitle,
                ServiceRequest = x.ServiceRequest,
                EquipmentRequest = x.EquipmentRequest,
                RegionId = x.RegionId,
                RegionCode = x.Region.RegionCode,
                RegionName = x.Region.RegionName,
                Deposit = x.Deposit,
                InstallationAddress = x.InstallationAddress,
                IsResponse = x.IsResponse,
                FullName = x.Customer.FullName,
                Gender = x.Customer.Gender,
                DateOfBirth = x.Customer.DateOfBirth,
                Address = x.Customer.Address,
                Email = x.Customer.Email,
                PhoneNumber = x.Customer.PhoneNumber,
                DateCreate = x.DateCreate,
            }).ToList();
            var response = new ApiResponse(StatusCodes.Status200OK, "Get Requests of CustomerID:"+ cusID + " successfully", customerRequestList);
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
                    // Gọi dịch vụ PayPal để tạo thanh toán và lấy thông tin đơn hàng
                    var order = await _payPalService.CreatePayment(cusReq.Deposit);

                    // Lấy URL phê duyệt từ PayPal
                    // (nơi người dùng sẽ được chuyển hướng để xác nhận thanh toán)
                    var approvalLink = order.Links?.FirstOrDefault(link => link.Rel == "approve")?.Href;
                    cusReq.DepositPaymentId = order.Id;
                    await _dbContext.CustomerRequests.AddAsync(cusReq);
                    await _dbContext.SaveChangesAsync();
                    //response = new ApiResponse(StatusCodes.Status200OK, "Create customer request successfully", cusReq);
                    return Created("success", new { data = cusReq, approvalUrl = approvalLink });
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

        [HttpPost("capture-deposit")]
        public async Task<IActionResult> CapturePayment([FromBody] PayPalCaptureDepositRequest captureRequest)
        {
            try
            {
                // Gọi dịch vụ PayPal để capture thanh toán dựa
                // trên orderId (mà người dùng đã xác nhận)
                var order = await _payPalService.CapturePayment(captureRequest.CustomerRequestId);
                
                // Kiểm tra trạng thái thanh toán
                // của đơn hàng (được hoàn thành hay chưa)
                bool isPaid = order.Status == "COMPLETED";

                // Cập nhật trạng thái đơn hàng trong cơ sở dữ liệu
                //await _dbContext.UpdateDepositStatus(captureRequest.OrderId, isPaid ? "Paid" : "Pending");
                var cusReq = await _dbContext.CustomerRequests.FirstOrDefaultAsync(o => o.DepositPaymentId.ToString() == captureRequest.CustomerRequestId);
                if (cusReq != null)
                {
                    cusReq.DepositStatus = isPaid ? "Paid" : "Pending";
                    await _dbContext.SaveChangesAsync();
                }
                // Trả về thông tin đơn hàng đã được capture
                return Ok(order);
            }
            catch (Exception ex)
            {
                // Nếu có lỗi xảy ra, trả về mã lỗi 400 và thông báo lỗi
                return BadRequest(new { message = ex.Message });
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

                if (existingReq == null)
                {
                    return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Invalid request ID", null));
                }

                // Nếu chưa phản hồi, thì đặt IsResponse = true
                if (!existingReq.IsResponse)
                {
                    existingReq.IsResponse = true;
                    existingReq.DateResolve = DateTime.Now; // Đánh dấu thời gian hoàn thành
                    await _dbContext.SaveChangesAsync();

                    return Ok(new ApiResponse(StatusCodes.Status200OK, "Request marked as resolved!", existingReq));
                }

                return Ok(new ApiResponse(StatusCodes.Status200OK, "Request was already resolved!", existingReq));
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
                .Include(x => x.Employee)
                .ToListAsync();

            var response = new ApiResponse(StatusCodes.Status200OK, "Get support requests successfully", supportRequests);
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
                var supReqStatus = !supportRequest.IsResolved;
                supportRequest.IsResolved = supReqStatus;
                supportRequest.DateResolved = supReqStatus ? DateTime.Now: null;
                supportRequest.EmpIdResolver = supReqStatus? empIdResolver:null;

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

        [HttpPost("support-response-mail")]
        public async Task<IActionResult> SupportResponseMail([FromBody] SupportRequestResponse SupReqRes)
        {
            if (SupReqRes == null || string.IsNullOrWhiteSpace(SupReqRes.ToEmail))
            {
                return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Invalid request", "Email is required"));
            }

            var supportRequest = await _dbContext.SupportRequests.FindAsync(SupReqRes.SupReqId);
            if (supportRequest == null)
            {
                return NotFound(new ApiResponse(StatusCodes.Status404NotFound, "Support request not found", null));
            }

            var emailRequest = new EmailRequest
            {
                ToMail = SupReqRes.ToEmail,
                Subject = SupReqRes.Subject,
                HtmlContent = $@"
        <html>
        <head>
            <style>
                .email-container {{
                    font-family: 'Arial', sans-serif;
                    line-height: 1.6;
                    color: #333333;
                    background-color: #f4f4f4;
                    width: 50%;
                    margin: 0 auto;
                    padding: 20px;
                    border: 1px solid #dddddd;
                    border-radius: 5px;
                    text-align: center;
                }}
                .header {{
                    background-color: #2c3e50;
                    color: #ffffff;
                    padding: 15px;
                    font-size: 24px;
                    font-weight: bold;
                    border-radius: 5px 5px 0 0;
                }}
                .content {{
                    padding: 15px;
                    text-align: left;
                    background-color: #ffffff;
                }}
                .content p {{
                    font-size: 16px;
                    color: #666666;
                }}
                .footer {{
                    font-size: 14px;
                    color: #999999;
                    text-align: center;
                    padding: 10px 0;
                    background-color: #2c3e50;
                    border-radius: 0 0 5px 5px;
                    color: #ffffff;
                }}
            </style>
        </head>
        <body>
            <div class='email-container'>
                <div class='header'>Support Response</div>
                <div class='content'>
                    <p>Dear {SupReqRes.CustomerName},</p>
                    <p>Thank you for reaching out to our support team.</p>
                    <p>{SupReqRes.ResponseContent}</p>
                    <p>Best regards,</p>
                    <p><strong>NEXUS Support Team</strong></p>
                </div>
                <div class='footer'>
                    &copy; 2024 NEXUS Company. All rights reserved.
                </div>
            </div>
        </body>
        </html>"
            };

            try
            {
                // Gửi email
                await _emailService.SendMailAsync(emailRequest);

                // Cập nhật vào database
                supportRequest.ResponseContent = SupReqRes.ResponseContent;
                supportRequest.DateResolved = DateTime.UtcNow;
                supportRequest.IsResolved = true;

                _dbContext.SupportRequests.Update(supportRequest);
                await _dbContext.SaveChangesAsync();

                return Ok(new ApiResponse(StatusCodes.Status200OK, "Email sent and response saved successfully", null));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ApiResponse(StatusCodes.Status500InternalServerError, "Error processing request", ex.Message));
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
