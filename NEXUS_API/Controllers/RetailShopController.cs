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
        private readonly IWebHostEnvironment _env;

        public RetailShopController(IRetailShopRepository retailShopRepository, IWebHostEnvironment env)
        {
            _retailShopRepository = retailShopRepository;
            _env = env;
        }

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

        [HttpPost]
        public async Task<IActionResult> AddRetailShop([FromForm] RetailShop retailShop, IFormFile imageFile)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new
                    {
                        message = "Invalid model state",
                        status = HttpStatusCode.BadRequest
                    });
                }

                // Kiểm tra nếu có file ảnh
                if (imageFile != null && imageFile.Length > 0)
                {
                    // Sử dụng thư mục con trong project hiện tại
                    var uploadsFolder = Path.Combine(@"D:\2308A0\HK3\API\NEXUS_API\NEXUS_API\NEXUS_API\Images\imageRetail");
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    // Tạo tên file duy nhất
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                    var filePath = Path.Combine(uploadsFolder, fileName);

                    // Lưu file ảnh vào thư mục
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(stream);
                    }

                    // Lưu đường dẫn ảnh vào database
                    retailShop.Image = "/uploads/imageRetail/" + fileName;
                }
                else
                {
                    return BadRequest(new { message = "Image file is required", status = HttpStatusCode.BadRequest });
                }

                // Lưu vào database
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



        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRetailShop(int id, [FromForm] RetailShop retailShop, IFormFile? imageFile)
        {
            try
            {
                if (id != retailShop.RetailShopId)
                    return BadRequest(new { message = "RetailShop ID mismatch.", status = HttpStatusCode.BadRequest });

                if (imageFile != null)
                {
                    // Sử dụng thư mục con trong project hiện tại
                    string uploadsFolder = Path.Combine(@"D:\2308A0\HK3\API\NEXUS_API\NEXUS_API\NEXUS_API\Images\imageRetail");
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                    string filePath = Path.Combine(uploadsFolder, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(stream);
                    }

                    retailShop.Image = "/uploads/imageRetail/" + fileName;
                }

                await _retailShopRepository.UpdateRetailShopAsync(retailShop);
                return Ok(new { data = retailShop, message = "RetailShop updated successfully", status = HttpStatusCode.OK });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message, status = HttpStatusCode.InternalServerError });
            }
        }
    }
}
