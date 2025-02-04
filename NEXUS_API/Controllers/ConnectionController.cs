using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NEXUS_API.Data;
using NEXUS_API.DTOs;
using NEXUS_API.Helpers;
using NEXUS_API.Models;

namespace NEXUS_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConnectionController : ControllerBase
    {
        private readonly DatabaseContext _dbContext;

        public ConnectionController(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }
        [HttpGet("get-all-connections")]
        public async Task<ActionResult> GetAllConnections()
        {
            var data = await _dbContext.Connections.ToListAsync();
            var response = new ApiResponse(StatusCodes.Status200OK, "Get all successfully", data);
            return Ok(response);
        }
        [HttpGet("get-all-connectiondiary")]
        public async Task<ActionResult> GetAllConnectiondiarys()
        {
            var data = await _dbContext.ConnectionDiarys.ToListAsync();
            var response = new ApiResponse(StatusCodes.Status200OK, "Get all successfully", data);
            return Ok(response);
        }
    }
}
