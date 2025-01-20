using System.ComponentModel.DataAnnotations;

namespace NEXUS_API.DTOs
{
    public class UpdateEmployeeRoleDto
    {
        [Required]
        public int EmployeeId { get; set; }

        [Required]
        public int EmployeeRoleId { get; set; }
    }

}
