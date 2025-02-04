using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NEXUS_API.Data;
using NEXUS_API.DTOs;
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

        [HttpPost("create-service-bill/{orderId}")]
        public async Task<IActionResult> CreateServiceBill(string orderId, [FromBody] CreateServiceBillDTO billDto)
        {
            if (string.IsNullOrWhiteSpace(orderId))
            {
                return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "OrderId is required", null));
            }

            try
            {
                // ServiceOrder
                var serviceOrder = await _dbContext.ServiceOrders
                    .Include(so => so.PlanFee)
                    .FirstOrDefaultAsync(so => so.OrderId == orderId);

                if (serviceOrder == null)
                {
                    return NotFound(new ApiResponse(StatusCodes.Status404NotFound, "ServiceOrder not found", null));
                }

                var existingBill = await _dbContext.ServiceBills
                    .FirstOrDefaultAsync(sb => sb.ServiceOrderId == orderId && sb.FromDate == billDto.FromDate && sb.ToDate == billDto.ToDate);
                if (existingBill != null)
                {
                    return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "ServiceBill already exists for this period", null));
                }

                decimal subtotal = CalculateSubtotal(billDto, serviceOrder.PlanFee);
                decimal taxRate = 12.24m / 100; 
                decimal tax = subtotal * taxRate;
                decimal total = subtotal + tax;

                // ServiceBill
                var serviceBill = new ServiceBill
                {
                    Payer = billDto.Payer,
                    CreateDate = DateTime.UtcNow,
                    FromDate = billDto.FromDate,
                    ToDate = billDto.ToDate,
                    Tax = tax,
                    Total = total,
                    IsPay = false, 
                    ServiceOrderId = serviceOrder.OrderId
                };

                if (billDto.Details != null && billDto.Details.Any())
                {
                    serviceBill.ServiceBillDetails = billDto.Details.Select(d => new ServiceBillDetail
                    {
                        Deposit = d.Deposit,
                        Discount = d.Discount,
                        Rental = d.Rental,
                        RentalDiscount = d.RentalDiscount,
                        CallCharge = d.CallCharge,
                        CallChargeTime = d.CallChargeTime,
                        LocalCallCharge = d.LocalCallCharge,
                        LocalTime = d.LocalTime,
                        STDCallCharge = d.STDCallCharge,
                        STDTime = d.STDTime,
                        MessageMobileCharge = d.MessageMobileCharge,
                        MessageMobileTime = d.MessageMobileTime,
                        ServiceDiscount = d.ServiceDiscount
                    }).ToList();
                }

                _dbContext.ServiceBills.Add(serviceBill);
                await _dbContext.SaveChangesAsync();

                return Ok(new ApiResponse(StatusCodes.Status201Created, "ServiceBill and details created successfully", serviceBill));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(StatusCodes.Status500InternalServerError, ex.Message, null));
            }
        }
        private decimal CalculateSubtotal(CreateServiceBillDTO billDto, PlanFee? planFee)
        {
            if (billDto.Details != null && billDto.Details.Any())
            {
                return billDto.Details.Sum(d => d.Deposit + d.Rental + d.CallCharge + d.LocalCallCharge + d.STDCallCharge + d.MessageMobileCharge);
            }
            else if (planFee != null)
            {
                return planFee.Rental;
            }

            throw new Exception("Cannot calculate subtotal: No details or PlanFee available.");
        }

        [HttpGet("get-service-bill/{billId}")]
        public async Task<IActionResult> GetServiceBill(int billId)
        {
            try
            {
                var serviceBill = await _dbContext.ServiceBills
                    .Include(sb => sb.ServiceBillDetails)
                    .FirstOrDefaultAsync(sb => sb.BillId == billId);

                if (serviceBill == null)
                {
                    return NotFound(new ApiResponse(StatusCodes.Status404NotFound, "ServiceBill not found", null));
                }

                return Ok(new ApiResponse(StatusCodes.Status200OK, "ServiceBill retrieved successfully", serviceBill));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(StatusCodes.Status500InternalServerError, "Internal Server Error", null));
            }
        }

        [HttpPut("pay-service-bill/{billId}")]
        public async Task<IActionResult> PayServiceBill(int billId)
        {
            try
            {
                var serviceBill = await _dbContext.ServiceBills
                    .FirstOrDefaultAsync(sb => sb.BillId == billId);

                if (serviceBill == null)
                {
                    return NotFound(new ApiResponse(StatusCodes.Status404NotFound, "ServiceBill not found", null));
                }

                if (serviceBill.IsPay)
                {
                    return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "ServiceBill is already paid", null));
                }

                serviceBill.IsPay = true;
                serviceBill.PayDate = DateTime.UtcNow;

                _dbContext.ServiceBills.Update(serviceBill);
                await _dbContext.SaveChangesAsync();

                return Ok(new ApiResponse(StatusCodes.Status200OK, "ServiceBill paid successfully", serviceBill));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(StatusCodes.Status500InternalServerError, "Internal Server Error", null));
            }
        }

    }
}
