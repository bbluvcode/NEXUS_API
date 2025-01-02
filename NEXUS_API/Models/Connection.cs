using System.ComponentModel.DataAnnotations;

namespace NEXUS_API.Models
{
    public class Connection
    {
        [Key]
        public string ConnID { get; set; }
        [Required]
        public string OrderID { get; set; }
        public string EquipmentID { get; set; }
        public int NumberOfConn { get; set; }
        public DateTime DateCreate { get; set; }
        public bool IsActive { get; set; }
        public string? Desciption { get; set; }
    }
}
