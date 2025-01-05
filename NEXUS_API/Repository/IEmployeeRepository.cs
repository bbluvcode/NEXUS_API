using NEXUS_API.Models;

namespace NEXUS_API.Repository
{
    public interface IEmployeeRepository
    {
        Task<IEnumerable<Employee>> GetAllEmployeeAsync();
        Task<IEnumerable<EmployeeRole>> GetAllEmployeeRolesAsync();
        Task<Employee> GetEmployeeByIdAsync(int id);
        Task AddEmployeeAsync(Employee employee);
        Task EditRoleEmployeeAsync(Employee employee);
        Task EditStatusEmployeeAsync(Employee employee);
        Task<RetainShop> GetRetainShopByIdAsync(int id);
        Task<EmployeeRole> GetEmployeeRoleByIdAsync(int id);
    }
}