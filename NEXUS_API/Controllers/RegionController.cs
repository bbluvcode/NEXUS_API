using Microsoft.AspNetCore.Mvc;
using NEXUS_API.Models;
using NEXUS_API.Repository;
using NEXUS_API.Service;
using System.Net;
using System.Threading.Tasks;

namespace NEXUS_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionController : Controller
    {
        private readonly IRegionRepository _regionRepository;

        public RegionController(IRegionRepository regionRepository)
        {
            _regionRepository = regionRepository;
        }

        // Lấy danh sách tất cả các vùng
        [HttpGet]
        public async Task<IActionResult> GetAllRegions()
        {
            try
            {
                var regions = await _regionRepository.GetAllRegionsAsync();
                return Ok(new
                {
                    data = regions,
                    message = "Get regions successfully",
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

        // Lấy thông tin vùng theo ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRegionById(int id)
        {
            try
            {
                var region = await _regionRepository.GetRegionByIdAsync(id);
                if (region == null)
                    return NotFound(new
                    {
                        message = $"Region with ID {id} not found.",
                        status = HttpStatusCode.NotFound
                    });

                return Ok(new
                {
                    data = region,
                    message = "Region retrieved successfully",
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

        // Thêm mới vùng
        [HttpPost]
        public async Task<IActionResult> AddRegion([FromBody] Region region)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new
                    {
                        message = "Invalid model state",
                        status = HttpStatusCode.BadRequest
                    });

                await _regionRepository.AddRegionAsync(region);
                return CreatedAtAction(nameof(GetRegionById), new { id = region.RegionId }, new
                {
                    data = region,
                    message = "Region created successfully",
                    status = HttpStatusCode.Created
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

        // Cập nhật thông tin vùng
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRegion(int id, [FromBody] Region region)
        {
            try
            {
                if (id != region.RegionId)
                    return BadRequest(new
                    {
                        message = "Region ID mismatch.",
                        status = HttpStatusCode.BadRequest
                    });

                await _regionRepository.UpdateRegionAsync(region);
                return Ok(new
                {
                    data = region,
                    message = "Region updated successfully",
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
