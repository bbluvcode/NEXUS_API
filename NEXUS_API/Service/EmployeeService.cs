using Microsoft.EntityFrameworkCore;
using NEXUS_API.Data;
using NEXUS_API.Models;
using NEXUS_API.Repository;

namespace NEXUS_API.Service
{
    public class EmployeeService : IEmployeeRepository
    {
        private readonly DatabaseContext _dbContext;

        public EmployeeService(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        // Get all employees
        public async Task<IEnumerable<Employee>> GetAllEmployeeAsync()
        {
            return await _dbContext.Employees.ToListAsync();
        }
        public async Task<IEnumerable<EmployeeRole>> GetAllEmployeeRolesAsync()
        {
            return await _dbContext.EmployeeRoles.ToListAsync();
        }

        // Get employee by ID
        public async Task<Employee> GetEmployeeByIdAsync(int id)
        {
            return await _dbContext.Employees.FirstOrDefaultAsync(e => e.EmployeeId == id);
        }

        // Add new employee
        public async Task AddEmployeeAsync(Employee employee)
        {
            try
            {
                _dbContext.Employees.Add(employee);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Log lỗi để kiểm tra
                Console.WriteLine(ex.InnerException?.Message);
                throw;
            }
        }


        // Edit employee role
        public async Task EditRoleEmployeeAsync(Employee employee)
        {
            var existingEmployee = _dbContext.Employees.FirstOrDefault(e => e.EmployeeId == employee.EmployeeId);
            if (existingEmployee != null)
            {
                // Cập nhật EmployeeRoleId
                existingEmployee.EmployeeRoleId = employee.EmployeeRoleId;

                // Lưu thay đổi vào cơ sở dữ liệu
                await _dbContext.SaveChangesAsync();
            }
        }

        // Edit employee status
        public async Task EditStatusEmployeeAsync(Employee employee)
        {
            var existingEmployee = _dbContext.Employees.FirstOrDefault(e => e.EmployeeId == employee.EmployeeId);
            if (existingEmployee != null)
            {
                // Toggle the Status value
                existingEmployee.Status = !existingEmployee.Status;
                await _dbContext.SaveChangesAsync();
            }
        }
        public async Task<RetainShop> GetRetainShopByIdAsync(int id)
        {
            return await _dbContext.RetainShops.FirstOrDefaultAsync(r => r.RetainShopId == id);
        }
        public async Task<EmployeeRole> GetEmployeeRoleByIdAsync(int id)
        {
            return await _dbContext.EmployeeRoles.FirstOrDefaultAsync(r => r.RoleId == id);
        }
    }
}
