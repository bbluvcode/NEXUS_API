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
        public async Task EditRoleEmployeeByIdAsync(Employee employee)
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
        public async Task<RetailShop> GetRetailShopByIdAsync(int id)
        {
            return await _dbContext.RetailShops.FirstOrDefaultAsync(r => r.RetailShopId == id);
        }
        public async Task<EmployeeRole> GetEmployeeRoleByIdAsync(int id)
        {
            return await _dbContext.EmployeeRoles.FirstOrDefaultAsync(r => r.RoleId == id);
        }

        public async Task EditRoleEmployeeAsync(EmployeeRole employeeRole)
        {
            var existingRole = await _dbContext.EmployeeRoles
                .FirstOrDefaultAsync(r => r.RoleId == employeeRole.RoleId);

            if (existingRole == null)
            {
                throw new Exception("Role not found");
            }

            existingRole.RoleName = employeeRole.RoleName;

            await _dbContext.SaveChangesAsync();
        }

        public async Task AddEmployeeRoleAsync(EmployeeRole employeeRole)
        {
            // You can include any additional logic here before adding the role
            await _dbContext.EmployeeRoles.AddAsync(employeeRole);
            await _dbContext.SaveChangesAsync();
        }
    }
}
