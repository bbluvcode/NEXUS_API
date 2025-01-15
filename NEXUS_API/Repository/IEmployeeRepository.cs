using NEXUS_API.Models;

namespace NEXUS_API.Repository
{
    public interface IEmployeeRepository
    {
        Task<IEnumerable<Employee>> GetAllEmployeeAsync();
        Task<Employee> GetEmployeeByIdAsync(int id);
        Task AddEmployeeAsync(Employee employee);
        Task EditRoleEmployeeByIdAsync(Employee employee);
        Task EditStatusEmployeeAsync(Employee employee);
        Task<RetailShop> GetRetailShopByIdAsync(int id);
        Task<EmployeeRole> GetEmployeeRoleByIdAsync(int id);
        Task<IEnumerable<EmployeeRole>> GetAllEmployeeRolesAsync();
        Task EditRoleEmployeeAsync(EmployeeRole employeeRole);
        Task AddEmployeeRoleAsync(EmployeeRole employeeRole);
    }
}