using System.ComponentModel.DataAnnotations;

namespace NEXUS_API.Models
{
    public class InStockOrderDetail
    {
        [Key]
        public int ISODID { get; set; } 

        [Required]
        public int ISOID { get; set; } 

        [Required]
        public int EquipmentID { get; set; } 

        [Required]
        public int Quantity { get; set; } 

        [Required]
        public float Price { get; set; } 
    }
}
