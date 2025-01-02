using System.ComponentModel.DataAnnotations;

namespace NEXUS_API.Models
{
    public class EmployeeRole
    {
        public int RoleId { get; set; }
        [Required]
        public string RoleName { get; set; }

        //Relationship
        public ICollection<Employee>? Employees { get; set; }
    }
}
