using System.ComponentModel.DataAnnotations;

namespace NEXUS_API.Models
{
    public class Connection
    {
        [Key]
        public string ConnectionId { get; set; }
        [Required]
        public int OrderId { get; set; }
        public int EquipmentId { get; set; }
        public int NumberOfConnections { get; set; }
        public DateTime DateCreate { get; set; }
        public bool IsActive { get; set; }
        public string? Description { get; set; }
    }
}
