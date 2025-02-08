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
            var data = await _dbContext.Connections
                .Include(c => c.ConnectionDiaries) 
                .Select(c => new
                {
                    c.ConnectionId,
                    c.NumberOfConnections,
                    c.DateCreate,
                    c.IsActive,
                    c.Description,
                    c.ServiceOrderId,
                    c.PlanId,
                    c.EquipmentId,
                    FirstConnectionDiary = c.ConnectionDiaries.OrderBy(cd => cd.DiaryId).FirstOrDefault() 
                })
                .ToListAsync();

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

        [HttpGet("get-diary/{connectionId}")]
        public async Task<ActionResult> GetConnectionDiary(string connectionId)
        {
            var connectionDiaries = await _dbContext.ConnectionDiaries
                .Where(cd => cd.ConnectionId == connectionId)
                .Distinct()
                .ToListAsync();

            if (connectionDiaries == null || !connectionDiaries.Any())
            {
                return NotFound(new ApiResponse(StatusCodes.Status404NotFound, "No diary entries found for the given Connection ID", null));
            }

            var response = new ApiResponse(StatusCodes.Status200OK, "Get diaries successfully", connectionDiaries);
            return Ok(response);
        }

    }
}
