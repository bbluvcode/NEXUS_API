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
    public class AccountController : ControllerBase
    {
        private readonly DatabaseContext _dbContext;

        public AccountController(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }
        [HttpGet]
        public async Task<IActionResult> GetAccounts()
        {
            var Accounts = await _dbContext.Accounts.ToListAsync();
            var response = new ApiResponse(StatusCodes.Status200OK, "get Accounts successfully", Accounts);
            return Ok(response);
        }
        [HttpGet("connections-by-account/{accountId}")]
        public async Task<IActionResult> GetConnectionsByAccountId(string accountId)
        {
            var connections = await _dbContext.ServiceOrders
                .Where(o => o.AccountId == accountId)
                .SelectMany(o => o.Connections)
                .Select(c => new
                {
                    OrderId = c.ServiceOrderId,
                    ConnectionId = c.ConnectionId,
                    IsActive = c.IsActive,
                    ConnectionDiaryDateStart = c.ConnectionDiaries
                        .OrderBy(cd => cd.DateStart)
                        .Select(cd => cd.DateStart)
                        .FirstOrDefault(),
                    ConnectionDiaryDateEnd = c.ConnectionDiaries
                        .OrderBy(cd => cd.DateEnd)
                        .Select(cd => cd.DateEnd)
                        .FirstOrDefault()
                })
                .ToListAsync();

            if (!connections.Any())
            {
                return NotFound(new ApiResponse(StatusCodes.Status404NotFound, "No connections found for the given account", null));
            }

            var response = new ApiResponse(StatusCodes.Status200OK, "Connections retrieved successfully", connections);
            return Ok(response);
        }
    }
}