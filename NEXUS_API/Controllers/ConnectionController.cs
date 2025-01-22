using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NEXUS_API.Data;
using NEXUS_API.DTOs;
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
        [HttpPost("setup-connection")]
        public async Task<IActionResult> SetupConnection([FromBody] ConnectionDTO connectionDTO)
        {
            var connection = new Connection
            {
                ServiceOrderId = connectionDTO.OrderId,
                EquipmentId = connectionDTO.EquipmentId,
                PlanId = connectionDTO.PlanId,
                NumberOfConnections = connectionDTO.NumberOfConnections,
                DateCreate = DateTime.UtcNow,
                IsActive = true
            };
            _dbContext.Connections.Add(connection);
            await _dbContext.SaveChangesAsync();
            return Ok(connection);
        }
    }
}
