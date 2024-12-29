using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace NEXUS_API.Models
{
    public class InStockRequestDetail
    {
        [Key]
        public int ISRDID { get; set; } 
        [Required]
        public int ISRID { get; set; } 

        [Required]
        public int EquipmentID { get; set; } 

        [Required]
        public int Quantity { get; set; }
    }
}
