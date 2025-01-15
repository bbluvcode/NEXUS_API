using Microsoft.AspNetCore.Mvc;
using NEXUS_API.Models;
using NEXUS_API.Repository;
using System.Net;
using System.Threading.Tasks;

namespace NEXUS_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RetailShopController : ControllerBase
    {
        private readonly IRetailShopRepository _retailShopRepository;

        public RetailShopController(IRetailShopRepository retailShopRepository)
        {
            _retailShopRepository = retailShopRepository;
            //man da ghe
        }

        // Lấy danh sách tất cả RetailShops
        [HttpGet]
        public async Task<IActionResult> GetAllRetailShops()
        {
            try
            {
                var retailShops = await _retailShopRepository.GetAllRetailShopsAsync();
                return Ok(new
                {
                    data = retailShops,
                    message = "Get retail shops successfully",
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

        // Lấy thông tin RetailShop theo ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRetailShopById(int id)
        {
            try
            {
                var retailShop = await _retailShopRepository.GetRetailShopByIdAsync(id);
                if (retailShop == null)
                    return NotFound(new
                    {
                        message = $"RetailShop with ID {id} not found.",
                        status = HttpStatusCode.NotFound
                    });

                return Ok(new
                {
                    data = retailShop,
                    message = "RetailShop retrieved successfully",
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

        // Thêm mới RetailShop
        [HttpPost]
        public async Task<IActionResult> AddRetailShop([FromBody] RetailShop retailShop)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new
                    {
                        message = "Invalid model state",
                        status = HttpStatusCode.BadRequest
                    });

                await _retailShopRepository.AddRetailShopAsync(retailShop);
                return CreatedAtAction(nameof(GetRetailShopById), new { id = retailShop.RetailShopId }, new
                {
                    data = retailShop,
                    message = "RetailShop created successfully",
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

        // Cập nhật thông tin RetailShop
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRetailShop(int id, [FromBody] RetailShop retailShop)
        {
            try
            {
                if (id != retailShop.RetailShopId)
                    return BadRequest(new
                    {
                        message = "RetailShop ID mismatch.",
                        status = HttpStatusCode.BadRequest
                    });

                await _retailShopRepository.UpdateRetailShopAsync(retailShop);
                return Ok(new
                {
                    data = retailShop,
                    message = "RetailShop updated successfully",
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
