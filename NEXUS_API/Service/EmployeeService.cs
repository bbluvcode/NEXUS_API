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

        // Get employee by ID
        public async Task<Employee> GetEmployeeByIdAsync(int id)
        {
            return await _dbContext.Employees.FirstOrDefaultAsync(e => e.EmpID == id);
        }

        // Add new employee
        public async Task AddEmployeeAsync(Employee employee)
        {
            _dbContext.Employees.Add(employee);
            await _dbContext.SaveChangesAsync();
        }

        // Edit employee role
        public async Task EditRoleEmployeeAsync(Employee employee)
        {
            var existingEmployee = _dbContext.Employees.FirstOrDefault(e => e.EmpID == employee.EmpID);
            if (existingEmployee != null)
            {
                existingEmployee.Role = employee.Role;
                await _dbContext.SaveChangesAsync();
            }
        }

        // Edit employee status
        public async Task EditStatusEmployeeAsync(Employee employee)
        {
            var existingEmployee = _dbContext.Employees.FirstOrDefault(e => e.EmpID == employee.EmpID);
            if (existingEmployee != null)
            {
                // Toggle the Status value
                existingEmployee.Status = !existingEmployee.Status;
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
