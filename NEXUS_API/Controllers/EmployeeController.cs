using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NEXUS_API.Data;
using NEXUS_API.DTOs;
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

        // Lấy danh sách tất cả nhân viên
        [HttpGet("role")]
        public async Task<IActionResult> GetAllEmployeeRole()
        {
            try
            {
                var employeeRoles = await _employeeRepository.GetAllEmployeeRolesAsync();
                return Ok(new
                {
                    data = employeeRoles,
                    message = "get employeeRoles successfully",
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
                // Kiểm tra model state
                if (!ModelState.IsValid)
                {
                    return BadRequest(new
                    {
                        message = "Invalid model state",
                        status = HttpStatusCode.BadRequest
                    });
                }

                // Kiểm tra RetainShopId có tồn tại trong bảng RetainShops
                var retainShopExists = await _employeeRepository.GetRetainShopByIdAsync(employee.RetainShopId);
                if (retainShopExists == null)
                {
                    return NotFound(new
                    {
                        message = $"RetainShop with ID {employee.RetainShopId} does not exist.",
                        status = HttpStatusCode.NotFound
                    });
                }
                // Kiểm tra RoleID có tồn tại trong bảng RetainShops
                var roleExists = await _employeeRepository.GetEmployeeRoleByIdAsync(employee.EmployeeRoleId);
                if (roleExists == null)
                {
                    return NotFound(new
                    {
                        message = $"EmployeeRole with ID {employee.EmployeeRoleId} does not exist.",
                        status = HttpStatusCode.NotFound
                    });
                }

                // Thêm nhân viên
                await _employeeRepository.AddEmployeeAsync(employee);

                // Trả về thông tin nhân viên vừa thêm
                return CreatedAtAction(nameof(GetEmployeeById), new { id = employee.EmployeeId }, new
                {
                    data = employee,
                    message = "Employee created successfully",
                    status = HttpStatusCode.Created
                });
            }
            catch (Exception ex)
            {
                // Xử lý ngoại lệ
                return StatusCode(500, new
                {
                    message = ex.Message,
                    status = HttpStatusCode.InternalServerError
                });
            }
        }





        [HttpPut("{id}/role")]
        public async Task<IActionResult> UpdateEmployeeRole(int id, [FromBody] UpdateEmployeeRoleDto updateRoleDto)
        {
            if (id != updateRoleDto.EmployeeId)
            {
                return BadRequest(new
                {
                    message = "Employee ID mismatch.",
                    status = HttpStatusCode.BadRequest
                });
            }

            try
            {
                await _employeeRepository.EditRoleEmployeeAsync(new Employee
                {
                    EmployeeId = updateRoleDto.EmployeeId,
                    EmployeeRoleId = updateRoleDto.EmployeeRoleId
                });

                return Ok(new
                {
                    employeeId = updateRoleDto.EmployeeId,
                    roleId = updateRoleDto.EmployeeRoleId
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

                return Ok(new
                {
                    employeeId = employee.EmployeeId,
                    status = employee.Status
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
