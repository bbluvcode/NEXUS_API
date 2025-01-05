using Microsoft.AspNetCore.Mvc;
using NEXUS_API.Models;
using NEXUS_API.Repository;
using System.Net;
using System.Threading.Tasks;

namespace NEXUS_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RetainShopController : ControllerBase
    {
        private readonly IRetainShopRepository _retainShopRepository;

        public RetainShopController(IRetainShopRepository retainShopRepository)
        {
            _retainShopRepository = retainShopRepository;
        }

        // Lấy danh sách tất cả RetainShops
        [HttpGet]
        public async Task<IActionResult> GetAllRetainShops()
        {
            try
            {
                var retainShops = await _retainShopRepository.GetAllRetainShopsAsync();
                return Ok(new
                {
                    data = retainShops,
                    message = "Get retain shops successfully",
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

        // Lấy thông tin RetainShop theo ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRetainShopById(int id)
        {
            try
            {
                var retainShop = await _retainShopRepository.GetRetainShopByIdAsync(id);
                if (retainShop == null)
                    return NotFound(new
                    {
                        message = $"RetainShop with ID {id} not found.",
                        status = HttpStatusCode.NotFound
                    });

                return Ok(new
                {
                    data = retainShop,
                    message = "RetainShop retrieved successfully",
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

        // Thêm mới RetainShop
        [HttpPost]
        public async Task<IActionResult> AddRetainShop([FromBody] RetainShop retainShop)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new
                    {
                        message = "Invalid model state",
                        status = HttpStatusCode.BadRequest
                    });

                await _retainShopRepository.AddRetainShopAsync(retainShop);
                return CreatedAtAction(nameof(GetRetainShopById), new { id = retainShop.RetainShopId }, new
                {
                    data = retainShop,
                    message = "RetainShop created successfully",
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

        // Cập nhật thông tin RetainShop
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRetainShop(int id, [FromBody] RetainShop retainShop)
        {
            try
            {
                if (id != retainShop.RetainShopId)
                    return BadRequest(new
                    {
                        message = "RetainShop ID mismatch.",
                        status = HttpStatusCode.BadRequest
                    });

                await _retainShopRepository.UpdateRetainShopAsync(retainShop);
                return Ok(new
                {
                    data = retainShop,
                    message = "RetainShop updated successfully",
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
