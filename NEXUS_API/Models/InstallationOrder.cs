using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NEXUS_API.Models
{
    public class InstallationOrder
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int InstallationId { get; set; }
        [Required]
        public string ServiceOrderId { get; set; } 
        public ServiceOrder ServiceOrder { get; set; }
        [Required]
        public int TechnicianId { get; set; } 
        public Employee Technician { get; set; }
        public DateTime DateAssigned { get; set; } 
        public DateTime? DateCompleted { get; set; } 
        public string Status { get; set; } // "Assigned", "InProgress", "Completed"
        public string? Notes { get; set; } 
    }

}
