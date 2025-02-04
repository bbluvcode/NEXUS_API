using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NEXUS_API.Data;
using NEXUS_API.Models;

namespace NEXUS_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KeywordController : ControllerBase
    {
        private readonly DatabaseContext _dbContext;
        public KeywordController(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }
        [HttpGet]
        public async Task<IActionResult> Getkeywords()
        {
            try
            {
                var keywords = await _dbContext.Keywords.ToListAsync();
                return Ok(keywords);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
        [HttpPost]
        public async Task<IActionResult> Addkeyword([FromBody] Keyword keyword)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _dbContext.Keywords.AddAsync(keyword);
                    await _dbContext.SaveChangesAsync();
                    return Created("success", keyword);
                }
                return BadRequest("bad request");

            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }

        }
    }

}
