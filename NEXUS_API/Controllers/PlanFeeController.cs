using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NEXUS_API.Data;
using NEXUS_API.Models;

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
        public async Task<ActionResult<IEnumerable<PlanFee>>> GetPlanFees()
        {
            return await _context.PlanFees.Include(pf => pf.Plan).ToListAsync();
        }

        // GET: api/PlanFee/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<PlanFee>> GetPlanFee(int id)
        {
            var planFee = await _context.PlanFees.Include(pf => pf.Plan).FirstOrDefaultAsync(pf => pf.PlanFeeId == id);

            if (planFee == null)
            {
                return NotFound(new { message = "PlanFee not found" });
            }

            return planFee;
        }

        // POST: api/PlanFee
        [HttpPost]
        public async Task<ActionResult<PlanFee>> CreatePlanFee(PlanFee planFee)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.PlanFees.Add(planFee);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPlanFee), new { id = planFee.PlanFeeId }, planFee);
        }

        // PUT: api/PlanFee/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePlanFee(int id, PlanFee planFee)
        {
            if (id != planFee.PlanFeeId)
            {
                return BadRequest(new { message = "PlanFee ID mismatch" });
            }

            _context.Entry(planFee).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PlanFeeExists(id))
                {
                    return NotFound(new { message = "PlanFee not found" });
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/PlanFee/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePlanFee(int id)
        {
            var planFee = await _context.PlanFees.FindAsync(id);
            if (planFee == null)
            {
                return NotFound(new { message = "PlanFee not found" });
            }

            _context.PlanFees.Remove(planFee);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PlanFeeExists(int id)
        {
            return _context.PlanFees.Any(pf => pf.PlanFeeId == id);
        }
    }
}
