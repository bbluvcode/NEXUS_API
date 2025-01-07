using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NEXUS_API.Models
{
    public class InStockOrderDetail
    {
        [Key]
        public int InStockOrderDetailId { get; set; } 
        [Required]
        public int Quantity { get; set; }
        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Price { get; set; }
        //Relationship
        [Required]
        public int InStockOrderId { get; set; }
        public InStockOrder? InStockOrder { get; set; }
        [Required]
        public int EquipmentId { get; set; }
        public Equipment? Equipment { get; set; }
    }
}
