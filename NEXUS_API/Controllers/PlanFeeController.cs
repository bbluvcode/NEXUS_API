using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Threading.Tasks;
using NEXUS_API.Models;
using NEXUS_API.Helpers;
using NEXUS_API.Data;

namespace NEXUS_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlanFeeController : ControllerBase
    {
        private readonly DatabaseContext _context;

        public PlanFeeController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: api/PlanFee
        [HttpGet]
        public async Task<IActionResult> GetPlanFees()
        {
            var planFees = await _context.PlanFees.Include(pf => pf.Plan).ToListAsync();
            var response = new ApiResponse(StatusCodes.Status200OK, "Get plan fees successfully", planFees);
            return Ok(response);
        }

        // GET: api/PlanFee/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPlanFee(int id)
        {
            var planFee = await _context.PlanFees.Include(pf => pf.Plan).FirstOrDefaultAsync(pf => pf.PlanFeeId == id);
            if (planFee == null)
            {
                return NotFound(new ApiResponse(StatusCodes.Status404NotFound, "PlanFee not found", null));
            }

            return Ok(new ApiResponse(StatusCodes.Status200OK, "Get plan fee successfully", planFee));
        }

        // POST: api/PlanFee
        [HttpPost]
        public async Task<IActionResult> CreatePlanFee([FromBody] PlanFee planFee)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _context.PlanFees.AddAsync(planFee);
                    await _context.SaveChangesAsync();
                    return CreatedAtAction(nameof(GetPlanFee), new { id = planFee.PlanFeeId },
                        new ApiResponse(StatusCodes.Status201Created, "Create plan fee successfully", planFee));
                }
                return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Bad request", null));
            }
            catch (Exception)
            {
                return StatusCode(500, new ApiResponse(StatusCodes.Status500InternalServerError, "Server error", null));
            }
        }

        // PUT: api/PlanFee/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePlanFee(int id, [FromBody] PlanFee planFeeUpdate)
        {
            try
            {
                var planFee = await _context.PlanFees.FirstOrDefaultAsync(pf => pf.PlanFeeId == id);
                if (planFee == null)
                {
                    return NotFound(new ApiResponse(StatusCodes.Status404NotFound, "PlanFee not found", null));
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Bad request", null));
                }

                planFee.PlanFeeName = planFeeUpdate.PlanFeeName;
                planFee.Description = planFeeUpdate.Description;
                planFee.IsUsing = planFeeUpdate.IsUsing;
                planFee.Rental = planFeeUpdate.Rental;
                planFee.CallCharge = planFeeUpdate.CallCharge;
                planFee.DTDCallCharge = planFeeUpdate.DTDCallCharge;
                planFee.MessageMobileCharge = planFeeUpdate.MessageMobileCharge;
                planFee.LocalCallCharge = planFeeUpdate.LocalCallCharge;

                await _context.SaveChangesAsync();
                return Ok(new ApiResponse(StatusCodes.Status200OK, "Update plan fee successfully", planFee));
            }
            catch (Exception)
            {
                return StatusCode(500, new ApiResponse(StatusCodes.Status500InternalServerError, "Server error", null));
            }
        }

        [HttpPatch("{id}/isusing")]
        public async Task<IActionResult> ChangePlanFeeIsUsing(int id, [FromBody] bool isUsing)
        {
            try
            {
                var planFee = await _context.PlanFees.FirstOrDefaultAsync(pf => pf.PlanFeeId == id);
                if (planFee == null)
                {
                    return NotFound(new
                    {
                        message = $"Plan fee with ID {id} not found.",
                        status = HttpStatusCode.NotFound
                    });
                }

                planFee.IsUsing = isUsing;
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    data = planFee,
                    message = "Plan fee status updated successfully",
                    status = HttpStatusCode.OK
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = ex.Message,
                    status = HttpStatusCode.InternalServerError
                });
            }
        }

    }
}
