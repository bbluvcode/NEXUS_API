using System.ComponentModel.DataAnnotations;

namespace NEXUS_API.Models
{
    public class OutStockOrderDetail
    {
        [Key]
        public int OSDID { get; set; } 

        [Required]
        public int OSID { get; set; } 

        [Required]
        public int EquipmentID { get; set; } 

        [Required]
        public int Quantity { get; set; } 

        [Required]
        public float Price { get; set; } 
    }
}
