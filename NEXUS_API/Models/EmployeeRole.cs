using System.ComponentModel.DataAnnotations;

namespace NEXUS_API.Models
{
    public class EmployeeRole
    {
        public int RoleID { get; set; }
        [Required]
        public string RoleName { get; set; }
    }
}
