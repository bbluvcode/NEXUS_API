using Azure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NEXUS_API.Data;
using NEXUS_API.DTOs;
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
                        FullName = e.FullName + " - " + e.EmployeeRole.RoleName + " - " + e.RetailShop.RetailShopName
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
                var customerRequest = await _dbContext.CustomerRequests.FirstOrDefaultAsync(cr => cr.RequestId == requestId);

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
                    await _dbContext.SaveChangesAsync();

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
                var serviceOrder = await _dbContext.ServiceOrders
                    .FirstOrDefaultAsync(so => so.OrderId == orderId);

                if (serviceOrder == null)
                {
                    return NotFound(new ApiResponse(StatusCodes.Status404NotFound, "ServiceOrder not found", null));
                }

                if (serviceOrder.SurveyStatus != "Installation")
                {
                    return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Survey must be valid to assign technician", null));
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
                if (serviceOrder != null)
                {
                    serviceOrder.SurveyStatus = "Installation Completed";
                    _dbContext.ServiceOrders.Update(serviceOrder);
                }

                await _dbContext.SaveChangesAsync();

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

                await _dbContext.SaveChangesAsync();

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