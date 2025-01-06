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
        [HttpGet("list")]
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
                var response = new ApiResponse(StatusCodes.Status404NotFound, "PlanFee not found", null);
                return NotFound(response);
            }

            var successResponse = new ApiResponse(StatusCodes.Status200OK, "Get plan fee successfully", planFee);
            return Ok(successResponse);
        }


        // POST: api/PlanFee
        [HttpPost]
        public async Task<IActionResult> CreatePlanFee(PlanFee planFee)
        {
            object response = null;
            try
            {
                if (ModelState.IsValid)
                {
                    _context.PlanFees.Add(planFee);
                    await _context.SaveChangesAsync();
                    response = new ApiResponse(StatusCodes.Status201Created, "Create plan fee successfully", planFee);
                    return CreatedAtAction(nameof(GetPlanFee), new { id = planFee.PlanFeeId }, response);
                }

                response = new ApiResponse(StatusCodes.Status400BadRequest, "Bad request", null);
                return BadRequest(response);
            }
            catch (Exception)
            {
                response = new ApiResponse(StatusCodes.Status500InternalServerError, "Server error", null);
                return StatusCode(500, response);
            }
        }

        // PUT: api/PlanFee/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePlanFee(int id, PlanFee planFee)
        {
            object response = null;
            try
            {
                if (id != planFee.PlanFeeId)
                {
                    response = new ApiResponse(StatusCodes.Status400BadRequest, "PlanFee ID mismatch", null);
                    return BadRequest(response);
                }

                _context.Entry(planFee).State = EntityState.Modified;

                await _context.SaveChangesAsync();
                response = new ApiResponse(StatusCodes.Status200OK, "Update plan fee successfully", planFee);
                return Ok(response);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PlanFeeExists(id))
                {
                    response = new ApiResponse(StatusCodes.Status404NotFound, "PlanFee not found", null);
                    return NotFound(response);
                }
                else
                {
                    response = new ApiResponse(StatusCodes.Status500InternalServerError, "Server error", null);
                    return StatusCode(500, response);
                }
            }
        }

        // DELETE: api/PlanFee/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePlanFee(int id)
        {
            object response = null;
            try
            {
                var planFee = await _context.PlanFees.FindAsync(id);
                if (planFee == null)
                {
                    response = new ApiResponse(StatusCodes.Status404NotFound, "PlanFee not found", null);
                    return NotFound(response);
                }

                _context.PlanFees.Remove(planFee);
                await _context.SaveChangesAsync();
                response = new ApiResponse(StatusCodes.Status200OK, "Delete plan fee successfully", planFee);
                return Ok(response);
            }
            catch (Exception)
            {
                response = new ApiResponse(StatusCodes.Status500InternalServerError, "Server error", null);
                return StatusCode(500, response);
            }
        }

        private bool PlanFeeExists(int id)
        {
            return _context.PlanFees.Any(pf => pf.PlanFeeId == id);
        }
    }
}
