using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace NEXUS_API.Models
{
    public class InStockRequestDetail
    {
        [Key]
        public int InStockRequestDetailId { get; set; } 
        [Required]
        public int InStockRequestId { get; set; } 

        [Required]
        public int EquipmentId { get; set; } 

        [Required]
        public int Quantity { get; set; }

        //Relationship
    }
}
