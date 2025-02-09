using Azure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using NEXUS_API.Data;
using NEXUS_API.DTOs;
using NEXUS_API.Helpers;
using NEXUS_API.Models;
using NEXUS_API.Service;
using PayPalCheckoutSdk.Orders;
using System;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace NEXUS_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ServiceOrderController : ControllerBase
    {
        private readonly DatabaseContext _dbContext;
        private readonly EmailService _emailService;

        public ServiceOrderController(DatabaseContext databaseContext, EmailService emailService)
        {
            _dbContext = databaseContext;
            _emailService = emailService;
        }

        [HttpGet]
        public async Task<ActionResult> GetServiceOrders()
        {
            var data = await _dbContext.ServiceOrders
                .Select(o => new
                {
                    o.OrderId,
                    o.DateCreate,
                    o.Deposit,
                    o.EmpIDSurveyor,
                    o.SurveyStatus,
                    InstallationOrderId = o.InstallationOrder != null ? o.InstallationOrder.InstallationId : (int?)null
                })
                .ToListAsync();

            var response = new ApiResponse(StatusCodes.Status200OK, "Get all successfully", data);
            return Ok(response);
        }


        [HttpGet("get-order/{id}")]
        public async Task<ActionResult> GetServiceOrder(string id)
        {
            object response = null;
            var serviceOrder = await _dbContext.ServiceOrders.FirstOrDefaultAsync(o => o.OrderId == id);
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
        public async Task<IActionResult> UpdateServiceOrder(string id, ServiceOrder serviceOrder)
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

        [HttpGet("get-surveyors/{requestId}")]
        public async Task<IActionResult> GetSurveyorsByRegion(int requestId)
        {
            try
            {
                var request = await _dbContext.CustomerRequests.FirstOrDefaultAsync(c => c.RequestId == requestId);

                if (request == null)
                {
                    return NotFound(new ApiResponse(StatusCodes.Status404NotFound, "Request not found", null));
                }

                int regionId = request.RegionId;

                var employees = await _dbContext.Employees
                    .Include(e => e.EmployeeRole)
                    .Include(e => e.RetailShop)
                    .Where(e => e.RetailShop.RegionId == regionId
                        && (e.EmployeeRole.RoleName == "Surveyor" || e.EmployeeRole.RoleName == "Technical"))
                    .Select(e => new
                    {
                        EmployeeId = e.EmployeeId,
                        FullName = e.FullName + " - " + e.EmployeeRole.RoleName + " - " + e.RetailShop.RetailShopName + " - Code: " + e.EmployeeCode
                    })
                    .ToListAsync();

                if (!employees.Any())
                {
                    return NotFound(new ApiResponse(StatusCodes.Status404NotFound, "No surveyors or technical staff found in this region.", null));
                }

                return Ok(new ApiResponse(StatusCodes.Status200OK, "Employees retrieved successfully", employees));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(StatusCodes.Status500InternalServerError, "Internal Server Error", null));
            }
        }

        [HttpPost("create-order-and-assign-survey/{requestId}")]
        public async Task<IActionResult> CreateServiceOrder(int requestId, [FromBody] AssignSurveyorDTO assignSurveyorDto)
        {
            try
            {
                //Find CustomerRequest from RequestId
                var customerRequest = await _dbContext.CustomerRequests
                    .Include(cr => cr.Customer)
                    .FirstOrDefaultAsync(cr => cr.RequestId == requestId);

                if (customerRequest == null)
                {
                    return NotFound(new ApiResponse(StatusCodes.Status404NotFound, "CustomerRequest not found", null));
                }

                // Create OrderId from Request
                string orderPrefix = (customerRequest.ServiceRequest + " " + customerRequest.RequestTitle).ToLower() switch
                {
                    var s when s.Contains("Dial-up", StringComparison.OrdinalIgnoreCase)
                          || s.Contains("Dialup", StringComparison.OrdinalIgnoreCase) => "D",
                    var s when s.Contains("Telephone", StringComparison.OrdinalIgnoreCase)
                          || s.Contains("Land-line", StringComparison.OrdinalIgnoreCase)
                          || s.Contains("Landline", StringComparison.OrdinalIgnoreCase) => "T",
                    var s when s.Contains("Broadband", StringComparison.OrdinalIgnoreCase)
                          || s.Contains("Broad-band", StringComparison.OrdinalIgnoreCase) => "B",
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

                // Find employee with role Surveyor
                var surveyor = await _dbContext.Employees.FirstOrDefaultAsync(e => e.EmployeeId == assignSurveyorDto.SurveyorId && (e.EmployeeRoleId == 6 || e.EmployeeRoleId == 5));
                if (surveyor == null)
                {
                    return NotFound(new ApiResponse(StatusCodes.Status404NotFound, "Surveyor not found or not eligible", null));
                }

                // Create ServiceOrder
                var serviceOrder = new ServiceOrder
                {
                    OrderId = orderId,
                    DateCreate = DateTime.UtcNow,
                    RequestId = customerRequest.RequestId,
                    SurveyStatus = "Surveyor Assigned",
                    Deposit = depositAmount,
                    EmpIDSurveyor = surveyor.EmployeeId,
                    EmpIDCreater = assignSurveyorDto.EmpIDCreater,
                };
                _dbContext.ServiceOrders.Add(serviceOrder);

                customerRequest.IsResponse = true;
                _dbContext.CustomerRequests.Update(customerRequest);

                await _dbContext.SaveChangesAsync();

                // Send email to customer
                var emailRequest = new EmailRequest
                {
                    ToMail = customerRequest.Customer.Email,
                    Subject = "Your Service Order Has Been Created",
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
                                    display: flex;
                                    align-items: center;
                                    justify-content: center;
                                    background-color: #2c3e50;
                                    color: #ffffff;
                                    padding: 15px;
                                    border-radius: 5px 5px 0 0;
                                }}
                                .header img {{
                                    width: 80px;
                                    height: 80px;
                                    border-radius: 50%;
                                }}
                                .header h2 {{
                                    text-align: center;
                                    font-size: 36px;
                                    font-weight: bold;
                                    margin-top: 5px;
                                    margin-left: 60px;
                                    color: #e74c3c;
                                }}
                                .content {{
                                    padding: 15px;
                                    background-color: #ffffff;
                                    text-align: left;
                                }}
                                .content p {{
                                    font-size: 16px;
                                    color: #666666;
                                }}
                                .footer {{
                                    font-size: 16px;
                                    color: #999999;
                                    text-align: center;
                                    padding: 10px 0;
                                    background-color: #2c3e50;
                                    border-radius: 0 0 5px 5px;
                                }}
                            </style>
                        </head>
                        <body>
                            <div class='email-container'>
                                <div class='header'>
                                    <img src='https://i.postimg.cc/sgyV5SqZ/logo-textwhite.png' alt='Company Logo'>
                                    <h2>Service Order Confirmation</h2>
                                </div>
                                <div class='content'>
                                    <p>Dear {customerRequest.Customer.FullName},</p>
                                    <p>Thank you for choosing us as your dedicated provider.</p>
                                    <p>We are extremely grateful that you took the time to send us your request about our Service.</p>
                                    <p>Your service order <strong>{orderId}</strong> has been successfully created.</p>
                                    <p>A surveyor has been assigned to evaluate your request.</p>
                                    <p>Deposit Amount: <strong>${depositAmount}</strong></p>
                                    <p>Surveyor Name: <strong>{surveyor.FullName}</strong></p>
                                    <p>We will keep you updated on further progress.</p>
                                    <p>Best regards,</p>
                                    <p>The NEXUS team</p>
                                </div>
                                <div class='footer'>
                                    <p>&copy; 2024 NEXUS Company. All rights reserved.</p>
                                </div>
                            </div>
                        </body>
                        </html>"
                };

                //Send email to surveyor
                var emailRequestToSurveyor = new EmailRequest
                {
                    ToMail = surveyor.Email,
                    Subject = "New Survey Assignment",
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
                                    display: flex;
                                    align-items: center;
                                    justify-content: center;
                                    background-color: #2c3e50;
                                    color: #ffffff;
                                    padding: 15px;
                                    border-radius: 5px 5px 0 0;
                                }}
                                .header img {{
                                    width: 80px;
                                    height: 80px;
                                    border-radius: 50%;
                                }}
                                .header h2 {{
                                    text-align: center;
                                    font-size: 36px;
                                    font-weight: bold;
                                    margin-top: 5px;
                                    margin-left: 60px;
                                    color: #e74c3c;
                                }}
                                .content {{
                                    padding: 15px;
                                    background-color: #ffffff;
                                    text-align: left;
                                }}
                                .content p {{
                                    font-size: 16px;
                                    color: #666666;
                                }}
                                .footer {{
                                    font-size: 16px;
                                    color: #999999;
                                    text-align: center;
                                    padding: 10px 0;
                                    background-color: #2c3e50;
                                    border-radius: 0 0 5px 5px;
                                }}
                            </style>
                        </head>
                        <body>
                            <div class='email-container'>
                                <div class='header'>
                                    <img src='https://i.postimg.cc/sgyV5SqZ/logo-textwhite.png' alt='Company Logo'>
                                    <h2>Service Order Confirmation</h2>
                                </div>
                                <div class='content'>
                                    <p>Dear {surveyor.FullName},</p>
                                    <p>You have been assigned to conduct a survey for the following service request:</p>
                                    <p><strong>Service Order ID:</strong> {orderId}</p>
                                    <p><strong>Customer Name:</strong> {customerRequest.Customer.FullName}</p>
                                    <p><strong>Address:</strong> {customerRequest.Customer.Address}</p>
                                    <p><strong>Phone Number:</strong> {customerRequest.Customer.PhoneNumber}</p>
                                    <p><strong>Request Details:</strong> {customerRequest.RequestTitle} - {customerRequest.ServiceRequest}</p>
                                    <p>Please schedule the survey as soon as possible.</p>
                                    <p>Best regards,</p>
                                    <p>The NEXUS team</p>
                                </div>
                                <div class='footer'>
                                    <p>&copy; 2024 NEXUS Company. All rights reserved.</p>
                                </div>
                            </div>
                        </body>
                        </html>"
                };
                try
                {
                    await _emailService.SendMailAsync(emailRequest);
                    await _emailService.SendMailAsync(emailRequestToSurveyor);
                }
                catch (Exception ex)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse(StatusCodes.Status500InternalServerError, "Error sending email: " + ex.Message, null));
                }

                return Created("success", new ApiResponse(StatusCodes.Status201Created, "ServiceOrder created and surveyor assigned successfully", serviceOrder));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(StatusCodes.Status500InternalServerError, "Internal Server Error " + ex.Message, null));
            }
        }

        [HttpGet("get-planfees/{orderId}")]
        public async Task<IActionResult> GetPlanFeesByOrderPrefix(string orderId)
        {
            try
            {
                string orderPrefix = orderId.Substring(0, 1);
                object plan = null;
                if (orderPrefix == "T")
                {
                    plan = await _dbContext.Plans.Where(p => p.PlanName.Contains("LandLine")).FirstOrDefaultAsync();

                }
                else
                {
                    plan = await _dbContext.Plans.Where(p => p.PlanName.StartsWith(orderPrefix)).FirstOrDefaultAsync();
                }

                if (plan == null)
                {
                    return NotFound(new ApiResponse(StatusCodes.Status404NotFound, "No plans found for the given orderPrefix", null));
                }

                var planFees = await _dbContext.PlanFees
                    .Where(pf => pf.PlanId == ((Plan)plan).PlanId)
                    .Select(pf => new
                    {
                        pf.PlanFeeId,
                        pf.PlanFeeName
                    })
                    .ToListAsync();

                if (planFees == null || planFees.Count == 0)
                {
                    return NotFound(new ApiResponse(StatusCodes.Status404NotFound, "No PlanFees found for the given plans", null));
                }

                return Ok(new ApiResponse(StatusCodes.Status200OK, "Plan fees fetched successfully", planFees));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(StatusCodes.Status500InternalServerError, "Internal Server Error", null));
            }
        }

        [HttpGet("get-equipment")]
        public async Task<IActionResult> GetEquipments()
        {
            try
            {
                var equipments = await _dbContext.Equipments
                    .Include(e => e.EquipmentType)
                    .Select(e => new
                    {
                        EquipmentId = e.EquipmentId,
                        EquipmentName = e.EquipmentName + " - " + e.EquipmentType.TypeName + " - " + e.Price
                    }).ToListAsync();

                if (!equipments.Any())
                {
                    return NotFound(new ApiResponse(StatusCodes.Status404NotFound, "Error", null));
                }

                return Ok(new ApiResponse(StatusCodes.Status200OK, "Get equipments successfully", equipments));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(StatusCodes.Status500InternalServerError, "Internal Server Error", null));
            }
        }

        [HttpGet("details/{orderId}")]
        public async Task<IActionResult> GetServiceOrderDetails(string orderId)
        {
            var serviceOrder = await _dbContext.ServiceOrders
                .Include(so => so.CustomerRequest)
                    .ThenInclude(cr => cr.Customer)
                .FirstOrDefaultAsync(so => so.OrderId == orderId);

            if (serviceOrder == null)
                return NotFound(new { message = "Service Order not found" });

            var bill = await _dbContext.ServiceBills.FirstOrDefaultAsync(c => c.ServiceOrderId == orderId);

            return Ok(new
            {
                customer = new
                {
                    FullName = serviceOrder.CustomerRequest.Customer.FullName,
                    PhoneNumber = serviceOrder.CustomerRequest.Customer.PhoneNumber,
                    InstallationAddress = serviceOrder.CustomerRequest.InstallationAddress ?? serviceOrder.CustomerRequest.Customer.Address,
                    Deposit = serviceOrder.Deposit.HasValue ? serviceOrder.Deposit.Value : (decimal?)null, 
                    Total = (bill != null && bill.Total.HasValue) ? bill.Total.Value : (decimal?)null,
                    IsPay = (bill != null && bill.IsPay) ? bill.Total.Value : (decimal?)null
                }
            });
        }

        [HttpPut("update-survey/{orderId}")]
        public async Task<IActionResult> UpdateSurveyStatusAndCreateAccount(string orderId, [FromBody] UpdateSurveyDTO updateSurveyDto)
        {
            try
            {
                var serviceOrder = await _dbContext.ServiceOrders
                    .Include(so => so.CustomerRequest)
                        .ThenInclude(cr => cr.Customer) 
                    .FirstOrDefaultAsync(so => so.OrderId == orderId);


                if (serviceOrder == null)
                {
                    return NotFound(new ApiResponse(StatusCodes.Status404NotFound, "ServiceOrder not found", null));
                }

                if (updateSurveyDto.SurveyStatus == "Valid")
                {
                    //Create AccountId
                    string accountPrefix = serviceOrder.OrderId.Substring(0, 1);
                    string regionCode = serviceOrder.CustomerRequest.RegionId.ToString("D3");
                    var lastAccount = await _dbContext.Accounts
                        .Where(a => a.AccountId.StartsWith(accountPrefix + regionCode))
                        .OrderByDescending(a => a.AccountId)
                        .FirstOrDefaultAsync();
                    int nextSerialNumberAccount = lastAccount == null
                        ? 1
                        : int.Parse(lastAccount.AccountId.Substring(4)) + 1;
                    string accountId = accountPrefix + regionCode + nextSerialNumberAccount.ToString("D12");

                    //Save Account
                    var account = new Account
                    {
                        AccountId = accountId,
                        CustomerId = serviceOrder.CustomerRequest.Customer.CustomerId,
                        Type = accountPrefix,
                        CreatedDate = DateTime.UtcNow,
                    };
                    _dbContext.Accounts.Add(account);
                    await _dbContext.SaveChangesAsync();

                    // Get PlanId from PlanFeeId
                    var planFee = await _dbContext.PlanFees
                        .Include(pf => pf.Plan)
                        .FirstOrDefaultAsync(pf => pf.PlanFeeId == updateSurveyDto.PlanFeeId);
                    if (planFee == null)
                    {
                        return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Invalid PlanFeeId", null));
                    }
                    serviceOrder.PlanFeeId = updateSurveyDto.PlanFeeId;

                    //Create ConnectionId
                    string connectionPrefix = serviceOrder.OrderId.Substring(0, 1); // D, B, T

                    var lastConnection = await _dbContext.Connections
                        .Where(c => c.ConnectionId.StartsWith(connectionPrefix + "-" + regionCode))
                        .OrderByDescending(c => c.ConnectionId)
                        .FirstOrDefaultAsync();

                    int nextSerialNumberConnection = lastConnection == null
                        ? 1
                        : int.Parse(lastConnection.ConnectionId.Split('-')[2]) + 1;

                    // Create ConnectionId
                    string connectionId = $"{connectionPrefix}-{regionCode}-{nextSerialNumberConnection:D6}";
                    var connections = new List<Connection>
                    {
                        new Connection
                        {
                            ConnectionId = connectionId,
                            NumberOfConnections = updateSurveyDto.NumberOfConnections ?? 0,
                            DateCreate = DateTime.UtcNow,
                            IsActive = false,
                            Description = "Connection pending activation",
                            ServiceOrderId = serviceOrder.OrderId,
                            PlanId = planFee.PlanId,
                            EquipmentId = updateSurveyDto.EquipmentId ?? null
                        }
                    };
                    _dbContext.Connections.AddRange(connections);

                    //Update ServiceOrder
                    serviceOrder.AccountId = account.AccountId;
                    serviceOrder.SurveyStatus = "Installation";
                    serviceOrder.SurveyDate = DateTime.UtcNow;
                    _dbContext.ServiceOrders.Update(serviceOrder);

                    //Create bill
                    decimal tax;
                    decimal totalAmount = CalculateBillAmount(serviceOrder, out tax);

                    var serviceBill = new ServiceBill
                    {
                        Payer = serviceOrder.CustomerRequest.Customer.FullName,
                        ServiceOrderId = serviceOrder.OrderId,
                        CreateDate = DateTime.UtcNow,
                        FromDate = DateTime.UtcNow,
                        ToDate = DateTime.UtcNow.AddMonths(1),
                        PayDate = null, // Chưa thanh toán
                        Tax = tax,
                        Total = totalAmount,
                        IsPay = false // Chưa thanh toán
                    };
                    _dbContext.ServiceBills.Add(serviceBill);

                    await _dbContext.SaveChangesAsync();

                    // Send email to customer
                    var emailRequest = new EmailRequest
                    {
                        ToMail = serviceOrder.CustomerRequest.Customer.Email,
                        Subject = "Survey Approved - Account Created",
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
                                    display: flex;
                                    align-items: center;
                                    justify-content: center;
                                    background-color: #2c3e50;
                                    color: #ffffff;
                                    padding: 15px;
                                    border-radius: 5px 5px 0 0;
                                }}
                                .header img {{
                                    width: 80px;
                                    height: 80px;
                                    border-radius: 50%;
                                }}
                                .header h2 {{
                                    text-align: center;
                                    font-size: 36px;
                                    font-weight: bold;
                                    margin-top: 5px;
                                    margin-left: 60px;
                                    color: #e74c3c;
                                }}
                                .content {{
                                    padding: 15px;
                                    background-color: #ffffff;
                                    text-align: left;
                                }}
                                .content p {{
                                    font-size: 16px;
                                    color: #666666;
                                }}
                                .footer {{
                                    font-size: 16px;
                                    color: #999999;
                                    text-align: center;
                                    padding: 10px 0;
                                    background-color: #2c3e50;
                                    border-radius: 0 0 5px 5px;
                                }}
                            </style>
                        </head>
                        <body>
                            <div class='email-container'>
                                <div class='header'>
                                    <img src='https://i.postimg.cc/sgyV5SqZ/logo-textwhite.png' alt='Company Logo'>
                                    <h2>Survey Approved</h2>
                                </div>
                                <div class='content'>
                                    <p>Dear {serviceOrder.CustomerRequest.Customer.FullName},</p>                               
                                    <p>Your service order <strong>{serviceOrder.OrderId}</strong> has been approved.</p>
                                    <p>Your account ID: <strong>{accountId}</strong></p>
                                    <p>Installation will proceed soon.</p>
                                    <p>Best regards,</p>
                                    <p>The NEXUS team</p>
                                </div>
                                <div class='footer'>
                                    <p>&copy; 2024 NEXUS Company. All rights reserved.</p>
                                </div>
                            </div>
                        </body>
                        </html>"
                    };
                    try
                    {
                        await _emailService.SendMailAsync(emailRequest);
                    }
                    catch (Exception ex)
                    {
                        return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse(StatusCodes.Status500InternalServerError, "Error sending email: " + ex.Message, null));
                    }

                    return Ok(new ApiResponse(StatusCodes.Status200OK, "Survey updated and account created successfully", serviceOrder));
                }
                else if (updateSurveyDto.SurveyStatus == "Invalid")
                {
                    if (string.IsNullOrWhiteSpace(updateSurveyDto.CancellationReason))
                    {
                        return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "CancellationReason is required for invalid surveys", null));
                    }

                    serviceOrder.SurveyStatus = "Cancelled";
                    serviceOrder.SurveyDate = DateTime.UtcNow;
                    serviceOrder.SurveyDescribe = updateSurveyDto.CancellationReason;
                    _dbContext.ServiceOrders.Update(serviceOrder);
                    await _dbContext.SaveChangesAsync();

                    // Send email to customer
                    var emailRequest = new EmailRequest
                    {
                        ToMail = serviceOrder.CustomerRequest.Customer.Email,
                        Subject = "Survey Rejected",
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
                                    display: flex;
                                    align-items: center;
                                    justify-content: center;
                                    background-color: #2c3e50;
                                    color: #ffffff;
                                    padding: 15px;
                                    border-radius: 5px 5px 0 0;
                                }}
                                .header img {{
                                    width: 80px;
                                    height: 80px;
                                    border-radius: 50%;
                                }}
                                .header h2 {{
                                    text-align: center;
                                    font-size: 36px;
                                    font-weight: bold;
                                    margin-top: 5px;
                                    margin-left: 60px;
                                    color: #e74c3c;
                                }}
                                .content {{
                                    padding: 15px;
                                    background-color: #ffffff;
                                    text-align: left;
                                }}
                                .content p {{
                                    font-size: 16px;
                                    color: #666666;
                                }}
                                .footer {{
                                    font-size: 16px;
                                    color: #999999;
                                    text-align: center;
                                    padding: 10px 0;
                                    background-color: #2c3e50;
                                    border-radius: 0 0 5px 5px;
                                }}
                            </style>
                        </head>
                        <body>
                            <div class='email-container'>
                                <div class='header'>
                                    <img src='https://i.postimg.cc/sgyV5SqZ/logo-textwhite.png' alt='Company Logo'>
                                    <h2>Survey Rejected</h2>
                                </div>
                                <div class='content'>
                                    <p>Dear {serviceOrder.CustomerRequest.Customer.FullName},</p>
                                    <p>Thanks a lot for sending us the details of request, it really sounds like a great opportunity!</p>
                                    <p>However, after reading through the proposal and having a discussion with my team, we’ve decided that unfortunately we won’t be able to take on this project.</p>
                                    <p>Though we’d love to move forward with this, at this moment we simply don’t have enough resources that this project deserves.</p>                                    
                                    <p>Should things change in the nearest future, I will definitely be in tough and let you know.</p>
                                    <p>Best regards,</p>
                                    <p>The NEXUS team</p>
                                </div>
                                <div class='footer'>
                                    <p>&copy; 2024 NEXUS Company. All rights reserved.</p>
                                </div>
                            </div>
                        </body>
                        </html>"
                    };
                    try
                    {
                        await _emailService.SendMailAsync(emailRequest);
                    }
                    catch (Exception ex)
                    {
                        return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse(StatusCodes.Status500InternalServerError, "Error sending email: " + ex.Message, null));
                    }

                    return Ok(new ApiResponse(StatusCodes.Status200OK, "Survey updated as cancelled", serviceOrder));
                }
                else
                {
                    serviceOrder.SurveyStatus = updateSurveyDto.SurveyStatus;
                    serviceOrder.SurveyDate = DateTime.UtcNow;
                    _dbContext.ServiceOrders.Update(serviceOrder);
                    await _dbContext.SaveChangesAsync();

                    return Ok(new ApiResponse(StatusCodes.Status200OK, "Survey updated successfully", serviceOrder));
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(StatusCodes.Status500InternalServerError, "Internal Server Error", null));
            }
        }

        private decimal CalculateBillAmount(ServiceOrder serviceOrder, out decimal tax)
        {
            decimal servicePrice = 0;
            decimal securityDeposit = 0;

            char orderType = serviceOrder.OrderId[0];

            switch (orderType)
            {
                case 'D': 
                    servicePrice = 50;
                    securityDeposit = 325;
                    break;
                case 'B': 
                    servicePrice = 225;
                    securityDeposit = 500;
                    break;
                case 'T': 
                    servicePrice = 75;
                    securityDeposit = 250;
                    break;
                default:
                    throw new Exception("Invalid service type in OrderId");
            }
            decimal subtotal = servicePrice  + securityDeposit;

            tax = subtotal * 0.1224M;

            return subtotal + tax;
        }

        [HttpGet("get-technicals/{orderId}")]
        public async Task<IActionResult> GetTechnicalsByRegion(string orderId)
        {
            try
            {
                var order = await _dbContext.ServiceOrders.FirstOrDefaultAsync(c => c.OrderId == orderId);

                if (order == null)
                {
                    return NotFound(new ApiResponse(StatusCodes.Status404NotFound, "Request not found", null));
                }

                var request = await _dbContext.CustomerRequests.FirstOrDefaultAsync(c => c.RequestId == order.RequestId);

                if (request == null)
                {
                    return NotFound(new ApiResponse(StatusCodes.Status404NotFound, "Request not found", null));
                }
                int regionId = request.RegionId;

                var technicals = await _dbContext.Employees
                    .Include(e => e.EmployeeRole)
                    .Include(e => e.RetailShop)
                    .Where(e => e.RetailShop.RegionId == regionId && e.EmployeeRole.RoleName == "Technical")
                    .Select(e => new
                    {
                        EmployeeId = e.EmployeeId,
                        FullName = e.FullName + " - " + e.EmployeeRole.RoleName + " - " + e.RetailShop.RetailShopName
                    })
                    .ToListAsync();

                if (technicals == null || !technicals.Any())
                {
                    return NotFound(new ApiResponse(StatusCodes.Status404NotFound, "No technical staff found in this region.", null));
                }

                return Ok(new ApiResponse(StatusCodes.Status200OK, "Technicals retrieved successfully", technicals));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(StatusCodes.Status500InternalServerError, "Internal Server Error", null));
            }
        }

        [HttpPost("assign-technician/{orderId}")]
        public async Task<IActionResult> AssignTechnician(string orderId, [FromBody] AssignTechnicianDTO assignDto)
        {
            try
            {
                var serviceOrder = await _dbContext.ServiceOrders.FirstOrDefaultAsync(so => so.OrderId == orderId);
                if (serviceOrder == null)
                {
                    return NotFound(new ApiResponse(StatusCodes.Status404NotFound, "ServiceOrder not found", null));
                }

                var request = await _dbContext.CustomerRequests.FirstOrDefaultAsync(c=>c.RequestId == serviceOrder.RequestId);
                if (request == null)
                {
                    return NotFound(new ApiResponse(StatusCodes.Status404NotFound, "request not found", null));
                }

                var customer = await _dbContext.Customers.FirstOrDefaultAsync(c=>c.CustomerId == request.CustomerId);
                if (customer == null)
                {
                    return NotFound(new ApiResponse(StatusCodes.Status404NotFound, "customer not found", null));
                }

                if (serviceOrder.SurveyStatus != "Installation")
                {
                    return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Survey must be valid to assign technician", null));
                }

                var technician = await _dbContext.Employees.FirstOrDefaultAsync(e => e.EmployeeId == assignDto.TechnicianId);
                if (technician == null)
                {
                    return NotFound(new ApiResponse(StatusCodes.Status404NotFound, "Technician not found", null));
                }

                // Create InstallationOrder
                var installationOrder = new InstallationOrder
                {
                    ServiceOrderId = serviceOrder.OrderId,
                    TechnicianId = assignDto.TechnicianId,
                    DateAssigned = DateTime.UtcNow,
                    Status = "Assigned"
                };
                _dbContext.InstallationOrders.Add(installationOrder);

                serviceOrder.SurveyStatus = "Technician Assigned";
                _dbContext.ServiceOrders.Update(serviceOrder);

                await _dbContext.SaveChangesAsync();

                //Send mail to Technician
                var emailRequestToTechnician = new EmailRequest
                {
                    ToMail = technician.Email,
                    Subject = "New Survey Assignment",
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
                                    display: flex;
                                    align-items: center;
                                    justify-content: center;
                                    background-color: #2c3e50;
                                    color: #ffffff;
                                    padding: 15px;
                                    border-radius: 5px 5px 0 0;
                                }}
                                .header img {{
                                    width: 80px;
                                    height: 80px;
                                    border-radius: 50%;
                                }}
                                .header h2 {{
                                    text-align: center;
                                    font-size: 36px;
                                    font-weight: bold;
                                    margin-top: 5px;
                                    margin-left: 60px;
                                    color: #e74c3c;
                                }}
                                .content {{
                                    padding: 15px;
                                    background-color: #ffffff;
                                    text-align: left;
                                }}
                                .content p {{
                                    font-size: 16px;
                                    color: #666666;
                                }}
                                .footer {{
                                    font-size: 16px;
                                    color: #999999;
                                    text-align: center;
                                    padding: 10px 0;
                                    background-color: #2c3e50;
                                    border-radius: 0 0 5px 5px;
                                }}
                            </style>
                        </head>
                        <body>
                            <div class='email-container'>
                                <div class='header'>
                                    <img src='https://i.postimg.cc/sgyV5SqZ/logo-textwhite.png' alt='Company Logo'>
                                    <h2>New Installation Assignment</h2>
                                </div>
                                <div class='content'>
                                    <p>Dear {technician.FullName},</p>
                                    <p>You have been assigned a new installation task:</p>
                                    <p><strong>Service Order ID:</strong> {serviceOrder.OrderId}</p>
                                    <p><strong>Customer Name:</strong> {customer.FullName}</p>
                                    <p><strong>Address:</strong> {customer.Address}</p>
                                    <p><strong>Phone Number:</strong> {customer.PhoneNumber}</p>
                                    <p><strong>Service Type:</strong> {request.ServiceRequest}</p>
                                    <p>Please proceed with the installation as scheduled.</p>
                                    <p>Best regards,</p>
                                    <p>The NEXUS team</p>
                                </div>
                                <div class='footer'>
                                    <p>&copy; 2024 NEXUS Company. All rights reserved.</p>
                                </div>
                            </div>
                        </body>
                        </html>"
                };
                try
                {
                    await _emailService.SendMailAsync(emailRequestToTechnician);
                }
                catch (Exception ex)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse(StatusCodes.Status500InternalServerError, "Error sending email: " + ex.Message, null));
                }

                return Ok(new ApiResponse(StatusCodes.Status200OK, "Technician assigned successfully", installationOrder));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(StatusCodes.Status500InternalServerError, "Internal Server Error", null));
            }
        }

        [HttpPut("complete-installation/{installationId}")]
        public async Task<IActionResult> CompleteInstallation(int installationId, [FromBody] CompleteInstallationDTO completeDto)
        {
            try
            {
                var installationOrder = await _dbContext.InstallationOrders
                    .Include(io => io.ServiceOrder)
                    .FirstOrDefaultAsync(io => io.InstallationId == installationId);

                if (installationOrder == null)
                {
                    return NotFound(new ApiResponse(StatusCodes.Status404NotFound, "InstallationOrder not found", null));
                }

                installationOrder.Status = "Completed";
                installationOrder.DateCompleted = DateTime.UtcNow;
                installationOrder.Notes = completeDto.Notes;
                _dbContext.InstallationOrders.Update(installationOrder);

                var serviceOrder = installationOrder.ServiceOrder;

                if (serviceOrder == null)
                {
                    return NotFound(new ApiResponse(StatusCodes.Status404NotFound, "serviceOrder not found", null));
                }

                if (serviceOrder != null)
                {
                    serviceOrder.SurveyStatus = "Installation Completed";
                    _dbContext.ServiceOrders.Update(serviceOrder);

                    var serviceBill = await _dbContext.ServiceBills
                            .FirstOrDefaultAsync(sb => sb.ServiceOrderId == serviceOrder.OrderId);

                    if (serviceBill != null && !serviceBill.IsPay)
                    {
                        serviceBill.IsPay = true;
                        serviceBill.PayDate = DateTime.UtcNow;
                        _dbContext.ServiceBills.Update(serviceBill);
                    }
                }

                await _dbContext.SaveChangesAsync();

                var request = await _dbContext.CustomerRequests.FirstOrDefaultAsync(c => c.RequestId == serviceOrder.RequestId);
                if (request == null)
                {
                    return NotFound(new ApiResponse(StatusCodes.Status404NotFound, "request not found", null));
                }

                var customer = await _dbContext.Customers.FirstOrDefaultAsync(c => c.CustomerId == request.CustomerId);
                if (customer == null)
                {
                    return NotFound(new ApiResponse(StatusCodes.Status404NotFound, "customer not found", null));
                }

                // Send email to customer
                var emailRequest = new EmailRequest
                {
                    ToMail = customer.Email,
                    Subject = "Installation Completed & Payment Confirmed",
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
                                    display: flex;
                                    align-items: center;
                                    justify-content: center;
                                    background-color: #2c3e50;
                                    color: #ffffff;
                                    padding: 15px;
                                    border-radius: 5px 5px 0 0;
                                }}
                                .header img {{
                                    width: 80px;
                                    height: 80px;
                                    border-radius: 50%;
                                }}
                                .header h2 {{
                                    text-align: center;
                                    font-size: 36px;
                                    font-weight: bold;
                                    margin-top: 5px;
                                    margin-left: 60px;
                                    color: #e74c3c;
                                }}
                                .content {{
                                    padding: 15px;
                                    background-color: #ffffff;
                                    text-align: left;
                                }}
                                .content p {{
                                    font-size: 16px;
                                    color: #666666;
                                }}
                                .footer {{
                                    font-size: 16px;
                                    color: #999999;
                                    text-align: center;
                                    padding: 10px 0;
                                    background-color: #2c3e50;
                                    border-radius: 0 0 5px 5px;
                                }}
                            </style>
                        </head>
                        <body>
                            <div class='email-container'>
                                <div class='header'>
                                    <img src='https://i.postimg.cc/sgyV5SqZ/logo-textwhite.png' alt='Company Logo'>
                                    <h2>Service Order Confirmation</h2>
                                </div>
                                <div class='content'>
                                    <p>Dear {customer.FullName},</p>
                                    <p>Thank you for choosing us as your dedicated provider.</p>
                                    <p>Your installation for service order <strong>{serviceOrder.OrderId}</strong> has been successfully completed.</p>
                                    <p>The installation was completed on: <strong>{installationOrder.DateCompleted}</strong>.</p>
                                    <p>Best regards,</p>
                                    <p>The NEXUS team</p>
                                </div>
                                <div class='footer'>
                                    <p>&copy; 2024 NEXUS Company. All rights reserved.</p>
                                </div>
                            </div>
                        </body>
                        </html>"
                };

                try
                {
                    await _emailService.SendMailAsync(emailRequest);
                }
                catch (Exception ex)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse(StatusCodes.Status500InternalServerError, "Error sending email: " + ex.Message, null));
                }

                return Ok(new ApiResponse(StatusCodes.Status200OK, "Installation completed successfully", installationOrder));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return StatusCode(500, new ApiResponse(StatusCodes.Status500InternalServerError, "Internal Server Error", null));
            }
        }

        [HttpPost("activate-connection/{orderId}")]
        public async Task<IActionResult> ActivateConnection(string orderId)
        {
            try
            {
                var serviceOrder = await _dbContext.ServiceOrders
                    .Include(so => so.InstallationOrder)
                    .Include(so => so.Connections)
                    .FirstOrDefaultAsync(so => so.OrderId == orderId);

                if (serviceOrder == null || serviceOrder.InstallationOrder?.Status != "Completed")
                {
                    return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Installation must be completed before activating connection", null));
                }

                var connection = serviceOrder.Connections.FirstOrDefault();
                if (connection == null)
                {
                    return NotFound(new ApiResponse(StatusCodes.Status404NotFound, "Connection not found", null));
                }

                // IsActive = true
                connection.IsActive = true;
                connection.Description = "Activate-connection";
                _dbContext.Connections.Update(connection);

                // ConnectionDiary
                var connectionDiary = new ConnectionDiary
                {
                    ConnectionId = connection.ConnectionId,
                    DateStart = DateTime.UtcNow
                };

                _dbContext.ConnectionDiaries.Add(connectionDiary);

                serviceOrder.SurveyStatus = "Activated Connection";
                _dbContext.ServiceOrders.Update(serviceOrder);

                await _dbContext.SaveChangesAsync();

                //Send mail to customer
                var request = await _dbContext.CustomerRequests.FirstOrDefaultAsync(c => c.RequestId == serviceOrder.RequestId);
                if (request == null)
                {
                    return NotFound(new ApiResponse(StatusCodes.Status404NotFound, "request not found", null));
                }

                var customer = await _dbContext.Customers.FirstOrDefaultAsync(c => c.CustomerId == request.CustomerId);
                if (customer == null)
                {
                    return NotFound(new ApiResponse(StatusCodes.Status404NotFound, "customer not found", null));
                }

                var emailRequest = new EmailRequest
                {
                    ToMail = customer.Email,
                    Subject = "Your Connection is Now Active",
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
                                    display: flex;
                                    align-items: center;
                                    justify-content: center;
                                    background-color: #2c3e50;
                                    color: #ffffff;
                                    padding: 15px;
                                    border-radius: 5px 5px 0 0;
                                }}
                                .header img {{
                                    width: 80px;
                                    height: 80px;
                                    border-radius: 50%;
                                }}
                                .header h2 {{
                                    text-align: center;
                                    font-size: 36px;
                                    font-weight: bold;
                                    margin-top: 5px;
                                    margin-left: 60px;
                                    color: #e74c3c;
                                }}
                                .content {{
                                    padding: 15px;
                                    background-color: #ffffff;
                                    text-align: left;
                                }}
                                .content p {{
                                    font-size: 16px;
                                    color: #666666;
                                }}
                                .footer {{
                                    font-size: 16px;
                                    color: #999999;
                                    text-align: center;
                                    padding: 10px 0;
                                    background-color: #2c3e50;
                                    border-radius: 0 0 5px 5px;
                                }}
                            </style>
                        </head>
                        <body>
                            <div class='email-container'>
                                <div class='header'>
                                    <img src='https://i.postimg.cc/sgyV5SqZ/logo-textwhite.png' alt='Company Logo'>
                                    <h2>Service Order Confirmation</h2>
                                </div>
                                <div class='content'>
                                    <p>Dear {customer.FullName},</p>
                                    <p>Thank you for choosing us as your dedicated provider.</p>
                                    <p>We are pleased to inform you that your connection for service order <strong>{serviceOrder.OrderId}</strong> has been successfully activated.</p>
                                    <p>You can now start using our service. If you have any issues, feel free to contact our support team.</p>
                                    <p>Best regards,</p>
                                    <p>The NEXUS team</p>
                                </div>
                                <div class='footer'>
                                    <p>&copy; 2024 NEXUS Company. All rights reserved.</p>
                                </div>
                            </div>
                        </body>
                        </html>"
                };

                try
                {
                    await _emailService.SendMailAsync(emailRequest);
                }
                catch (Exception ex)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse(StatusCodes.Status500InternalServerError, "Error sending email: " + ex.Message, null));
                }
                return Ok(new ApiResponse(StatusCodes.Status200OK, "Connection activated successfully", connection));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(StatusCodes.Status500InternalServerError, "Internal Server Error", null));
            }
        }

        [HttpPut("deactivate-connection/{connectionId}")]
        public async Task<IActionResult> DeactivateConnection(string connectionId)
        {
            try
            {
                var connection = await _dbContext.Connections
                    .Include(c => c.ConnectionDiaries)
                    .FirstOrDefaultAsync(c => c.ConnectionId == connectionId);

                if (connection == null)
                {
                    return NotFound(new ApiResponse(StatusCodes.Status404NotFound, "Connection not found", null));
                }

                var activeDiary = connection.ConnectionDiaries.FirstOrDefault(cd => cd.DateEnd == null);
                if (activeDiary == null)
                {
                    return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "No active connection found to deactivate", null));
                }

                activeDiary.DateEnd = DateTime.UtcNow;
                _dbContext.ConnectionDiaries.Update(activeDiary);
                await _dbContext.SaveChangesAsync();

                return Ok(new ApiResponse(StatusCodes.Status200OK, "Connection deactivated successfully", activeDiary));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(StatusCodes.Status500InternalServerError, "Internal Server Error", null));
            }
        }
    }
}