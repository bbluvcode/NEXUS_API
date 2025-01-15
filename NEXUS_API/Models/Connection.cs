using System.ComponentModel.DataAnnotations;

namespace NEXUS_API.Models
{
    public class Connection
    {
        [Key]
        public string ConnectionId { get; set; }
        public int NumberOfConnections { get; set; }
        public DateTime DateCreate { get; set; }
        public bool IsActive { get; set; }
        public string? Description { get; set; }

        //Relationship
        [Required]
        public int ServiceOrderId { get; set; }
        public ServiceOrder? ServiceOrder { get; set; }
        [Required]
        public int PlanId { get; set; }
        public Plan? Plan { get; set; }
        [Required]
        public int? EquipmentId { get; set; }
        public Equipment? Equipment { get; set; }
        public ICollection<ConnectionDiary>? ConnectionDiaries { get; set; }
    }
}
