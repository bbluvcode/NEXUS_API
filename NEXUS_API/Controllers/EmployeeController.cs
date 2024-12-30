using Microsoft.AspNetCore.Mvc;
using NEXUS_API.Models;
using NEXUS_API.Repository;
using NEXUS_API.Service;
using System.Net;
using System.Threading.Tasks;

namespace NEXUS_API.Controllers
{
    //man
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : Controller
    {
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeeController(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        // Lấy danh sách tất cả nhân viên
        [HttpGet]
        public async Task<IActionResult> GetAllEmployees()
        {
            try
            {
                var employees = await _employeeRepository.GetAllEmployeeAsync();
                return Ok(new
                {
                    data = employees,
                    message = "get employees successfully",
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

        // Lấy thông tin nhân viên theo ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmployeeById(int id)
        {
            try
            {
                var employee = await _employeeRepository.GetEmployeeByIdAsync(id);
                if (employee == null)
                    return NotFound(new
                    {
                        message = $"Employee with ID {id} not found.",
                        status = HttpStatusCode.NotFound
                    });

                return Ok(new
                {
                    data = employee,
                    message = "Employee retrieved successfully",
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

        // Thêm mới nhân viên
        [HttpPost]
        public async Task<IActionResult> AddEmployee([FromBody] Employee employee)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new
                    {
                        message = "Invalid model state",
                        status = HttpStatusCode.BadRequest
                    });

                await _employeeRepository.AddEmployeeAsync(employee);
                return CreatedAtAction(nameof(GetEmployeeById), new { id = employee.EmpID }, new
                {
                    data = employee,
                    message = "Employee created successfully",
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

        // Cập nhật vai trò của nhân viên
        [HttpPut("{id}/role")]
        public async Task<IActionResult> UpdateEmployeeRole(int id, [FromBody] Employee employee)
        {
            try
            {
                if (id != employee.EmpID)
                    return BadRequest(new
                    {
                        message = "Employee ID mismatch.",
                        status = HttpStatusCode.BadRequest
                    });

                await _employeeRepository.EditRoleEmployeeAsync(employee);
                return NoContent(); // Status 204
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

        // Chuyển đổi trạng thái công việc của nhân viên (toggle)
        [HttpPut("{id}/status")]
        public async Task<IActionResult> ToggleEmployeeStatus(int id)
        {
            try
            {
                var employee = await _employeeRepository.GetEmployeeByIdAsync(id);
                if (employee == null)
                    return NotFound(new
                    {
                        message = $"Employee with ID {id} not found.",
                        status = HttpStatusCode.NotFound
                    });

                await _employeeRepository.EditStatusEmployeeAsync(employee);
                return NoContent(); // Status 204
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
