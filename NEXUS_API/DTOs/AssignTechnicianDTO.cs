using System.ComponentModel.DataAnnotations;

namespace NEXUS_API.DTOs
{
    public class AssignTechnicianDTO
    {
        [Required]
        public int TechnicianId { get; set; }
    }
}
