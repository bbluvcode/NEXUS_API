using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NEXUS_API.Models
{
    public class OutStockOrderDetail
    {
        [Key]
        public int OutStockDetailId { get; set; } 

        [Required]
        public int OutStockId { get; set; } 

        [Required]
        public int EquipmentId { get; set; } 

        [Required]
        public int Quantity { get; set; } 

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Price { get; set; } 
    }
}
