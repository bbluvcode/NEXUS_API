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
    public class PlanController : ControllerBase
    {
        private readonly DatabaseContext _dbContext;

        public PlanController(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetPlans()
        {
            var plans = await _dbContext.Plans.ToListAsync();
            var response = new ApiResponse(StatusCodes.Status200OK, "get plans successfully", plans);
            return Ok(response);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPlanById(int id)
        {
            var plan = await _dbContext.Plans.FirstOrDefaultAsync(p => p.PlanId == id);

            if (plan == null)
            {
                // If no plan with the given ID is found, return a 404 response
                return NotFound(new ApiResponse(StatusCodes.Status404NotFound, "Plan not found!", null));
            }

            // If the plan is found, return it with a success response
            return Ok(new ApiResponse(StatusCodes.Status200OK, "Plan retrieved successfully", plan));
        }

        [HttpPost]
        public async Task<IActionResult> CreatePlan([FromBody] Plan plan)
        {
            object response = null;
            try
            {
                if (ModelState.IsValid)
                {
                    await _dbContext.Plans.AddAsync(plan);
                    await _dbContext.SaveChangesAsync();
                    response = new ApiResponse(StatusCodes.Status201Created, "create plan successfully", plan);
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
        public async Task<IActionResult> UpdatePlan(int id, [FromBody] Plan planUpdate)
        {
            object response = null;
            try
            {
                var plan = await _dbContext.Plans.FirstOrDefaultAsync(p => p.PlanId == id);
                if (plan == null)
                {
                    response = new ApiResponse(StatusCodes.Status404NotFound, "plan not found", null);
                    return NotFound(response);
                }

                if (!ModelState.IsValid)
                {
                    response = new ApiResponse(StatusCodes.Status400BadRequest, "bad request", null);
                    return BadRequest(response);
                }

                plan.PlanName = planUpdate.PlanName;
                plan.SecurityDeposit = planUpdate.SecurityDeposit;
                plan.Description = planUpdate.Description;
                plan.IsUsing = planUpdate.IsUsing;

                await _dbContext.SaveChangesAsync();
                response = new ApiResponse(StatusCodes.Status200OK, "update plan successfully", plan);
                return Ok(response);
            }
            catch (Exception)
            {
                response = new ApiResponse(StatusCodes.Status500InternalServerError, "server error", null);
                return StatusCode(500, response);
            }
        }

        [HttpPatch("{id}/isusing")]
        public async Task<IActionResult> ChangePlanIsUsing(int id, [FromBody] bool isUsing)
        {
            try
            {
                var plan = await _dbContext.Plans.FirstOrDefaultAsync(p => p.PlanId == id);
                if (plan == null)
                {
                    return NotFound(new
                    {
                        message = $"Plan with ID {id} not found.",
                        status = HttpStatusCode.NotFound
                    });
                }

                plan.IsUsing = isUsing;
                await _dbContext.SaveChangesAsync();

                return Ok(new
                {
                    data = plan,
                    message = "Plan status updated successfully",
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
