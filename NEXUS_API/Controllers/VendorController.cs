using Microsoft.AspNetCore.Mvc;
using NEXUS_API.Repository;
using System.Net;

[Route("api/[controller]")]
[ApiController]
public class VendorController : ControllerBase
{
    private readonly IVendorRepository _vendorRepository;

    public VendorController(IVendorRepository vendorRepository)
    {
        _vendorRepository = vendorRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllVendors()
    {
        try
        {
            var vendors = await _vendorRepository.GetAllVendorsAsync();
            return Ok(new
            {
                data = vendors,
                message = "Get vendors successfully",
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
    public async Task<IActionResult> GetVendorById(int id)
    {
        try
        {
            var vendor = await _vendorRepository.GetVendorByIdAsync(id);
            if (vendor == null)
            {
                return NotFound(new
                {
                    message = $"Vendor with ID {id} not found.",
                    status = HttpStatusCode.NotFound
                });
            }

            return Ok(new
            {
                data = vendor,
                message = "Vendor retrieved successfully",
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

    [HttpPatch("{id}/status")]
    public async Task<IActionResult> UpdateVendorStatus(int id, [FromBody] bool status)
    {
        try
        {
            await _vendorRepository.UpdateVendorStatusAsync(id, status);
            return Ok(new
            {
                message = "Vendor status updated successfully",
                status = HttpStatusCode.OK
            });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new
            {
                message = ex.Message,
                status = HttpStatusCode.NotFound
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
