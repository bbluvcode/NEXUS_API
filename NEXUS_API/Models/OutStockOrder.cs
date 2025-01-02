using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NEXUS_API.Models
{
    public class OutStockOrder
    {
        [Key]
        public int OutStockId { get; set; } 

        [Required]
        public int StockId { get; set; } 

        [Required]
        public int EmpployeeId { get; set; } 

        [Required]
        public DateTime CreateDate { get; set; } 

        [Required]
        public DateTime PayDate { get; set; } 

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Tax { get; set; } 

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Total { get; set; } 

        [Required]
        public bool isPay { get; set; }

        //Relationship
    }
}
