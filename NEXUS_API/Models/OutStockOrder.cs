using System.ComponentModel.DataAnnotations;

namespace NEXUS_API.Models
{
    public class OutStockOrder
    {
        [Key]
        public int OSID { get; set; } 

        [Required]
        public int StockID { get; set; } 

        [Required]
        public int EmpID { get; set; } 

        [Required]
        public DateTime CreateDate { get; set; } 

        [Required]
        public DateTime PayDate { get; set; } 

        [Required]
        public float Tax { get; set; } 

        [Required]
        public float Total { get; set; } 

        [Required]
        public bool isPay { get; set; } 
    }
}
