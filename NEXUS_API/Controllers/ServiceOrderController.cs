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
            var data =  await _dbContext.ServiceOrders.ToListAsync();
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
        [HttpPost("create-service-order")]
        public async Task<IActionResult> CreateServiceOrder([FromBody] ServiceOrderDTO orderDto)
        {
            var order = new ServiceOrder
            {
                AccountId = orderDto.AccountId,
                EmpIDCreater = orderDto.EmpIDCreater,
                DateCreate = DateTime.UtcNow,
                Deposit = orderDto.Deposit,
                EmpIDSurveyor = orderDto.EmpIDSurveyor,
                SurveyDate = orderDto.SurveyDate,
                SurveyStatus = "valid"
            };

            _dbContext.ServiceOrders.Add(order);
            await _dbContext.SaveChangesAsync();
            return Ok(order);
        }
        [HttpPut("complete-order/{orderId}")]
        public async Task<IActionResult> CompleteOrder(int orderId)
        {
            var order = await _dbContext.ServiceOrders.FindAsync(orderId);
            if (order == null)
                return NotFound("Order not found.");

            order.SurveyStatus = "finish";
            await _dbContext.SaveChangesAsync();
            return Ok("Order completed successfully.");
        }
        [HttpPost("assign-surveyor/{orderId}")]
        public async Task<IActionResult> AssignSurveyor(string orderId, [FromBody] AssignSurveyorDTO assignSurveyorDto)
        {
            if (string.IsNullOrWhiteSpace(orderId))
            {
                return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "OrderId is required", null));
            }

            try
            {
                var serviceOrder = await _dbContext.ServiceOrders.FirstOrDefaultAsync(so => so.OrderId == orderId);

                if (serviceOrder == null)
                {
                    return NotFound(new ApiResponse(StatusCodes.Status404NotFound, "ServiceOrder not found", null));
                }

                if (serviceOrder.SurveyStatus != "not yet")
                {
                    return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Survey already assigned or completed", null));
                }
                // employee role Surveyor
                var surveyor = await _dbContext.Employees.FirstOrDefaultAsync(e => e.EmployeeId == assignSurveyorDto.SurveyorId && e.EmployeeRoleId == 5);

                if (surveyor == null)
                {
                    return NotFound(new ApiResponse(StatusCodes.Status404NotFound, "Surveyor not found or not eligible", null));
                }

                // Update ServiceOrder
                serviceOrder.EmpIDCreater = assignSurveyorDto.EmpIDCreater;
                serviceOrder.EmpIDSurveyor = surveyor.EmployeeId;
                serviceOrder.EmployeeSurveyor = surveyor;
                serviceOrder.SurveyDate = DateTime.UtcNow;
                serviceOrder.SurveyStatus = "assigned";

                _dbContext.ServiceOrders.Update(serviceOrder);
                await _dbContext.SaveChangesAsync();

                return Ok(new ApiResponse(StatusCodes.Status200OK, "Surveyor assigned successfully", serviceOrder));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(StatusCodes.Status500InternalServerError, "Internal Server Error", null));
            }
        }
        [HttpPut("update-survey/{orderId}")]
        public async Task<IActionResult> UpdateSurveyStatusAndCreateAccount(string orderId, [FromBody] UpdateSurveyDTO updateSurveyDto)
        {
            if (string.IsNullOrWhiteSpace(orderId))
            {
                return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "OrderId is required", null));
            }

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

                if (updateSurveyDto.SurveyStatus == "valid")
                {
                    //Create AccountId
                    string accountPrefix = serviceOrder.OrderId.Substring(0, 1);
                    string regionCode = serviceOrder.CustomerRequest.RegionId.ToString("D3");
                    var lastAccount = await _dbContext.Accounts
                        .Where(a => a.AccountId.StartsWith(accountPrefix + regionCode))
                        .OrderByDescending(a => a.AccountId)
                        .FirstOrDefaultAsync();
                    int nextSerialNumber = lastAccount == null
                        ? 1
                        : int.Parse(lastAccount.AccountId.Substring(4)) + 1;
                    string accountId = accountPrefix + regionCode + nextSerialNumber.ToString("D12");

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

                    
                    serviceOrder.PlanFeeId = updateSurveyDto.PlanFeeId;
                    var connections = new List<Connection>
                    {
                        new Connection
                        {
                            ConnectionId = Guid.NewGuid().ToString(),
                            NumberOfConnections = updateSurveyDto.NumberOfConnections,
                            DateCreate = DateTime.UtcNow,
                            IsActive = false,
                            Description = "Connection pending activation",
                            ServiceOrderId = serviceOrder.OrderId,
                            PlanId = updateSurveyDto.PlanFeeId,
                            EquipmentId = updateSurveyDto.EquipmentId 
                        }
                    };
                    _dbContext.Connections.AddRange(connections);

                    //Update ServiceOrder
                    serviceOrder.AccountId = account.AccountId;
                    serviceOrder.SurveyStatus = "installation";
                    serviceOrder.SurveyDate = DateTime.UtcNow;
                    _dbContext.ServiceOrders.Update(serviceOrder);
                    await _dbContext.SaveChangesAsync();

                    return Ok(new ApiResponse(StatusCodes.Status200OK, "Survey updated and account created successfully", serviceOrder));
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
        [HttpPost("assign-technician/{orderId}")]
        public async Task<IActionResult> AssignTechnician(string orderId, [FromBody] AssignTechnicianDTO assignDto)
        {
            if (string.IsNullOrWhiteSpace(orderId))
            {
                return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "OrderId is required", null));
            }

            try
            {
                var serviceOrder = await _dbContext.ServiceOrders
                    .FirstOrDefaultAsync(so => so.OrderId == orderId);

                if (serviceOrder == null)
                {
                    return NotFound(new ApiResponse(StatusCodes.Status404NotFound, "ServiceOrder not found", null));
                }

                if (serviceOrder.SurveyStatus != "installation")
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
                    .FirstOrDefaultAsync(io => io.InstallationId == installationId);

                if (installationOrder == null)
                {
                    return NotFound(new ApiResponse(StatusCodes.Status404NotFound, "InstallationOrder not found", null));
                }

                installationOrder.Status = "Completed";
                installationOrder.DateCompleted = DateTime.UtcNow;
                installationOrder.Notes = completeDto.Notes;

                _dbContext.InstallationOrders.Update(installationOrder);
                await _dbContext.SaveChangesAsync();

                return Ok(new ApiResponse(StatusCodes.Status200OK, "Installation completed successfully", installationOrder));
            }
            catch (Exception ex)
            {
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
    }
}