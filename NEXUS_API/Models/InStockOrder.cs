using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NEXUS_API.Models
{
    public class InStockOrder
    {
        [Key]
        public int ISOID { get; set; } 

        [Required]
        public int ISRID { get; set; } 

        [Required]
        public int VendorID { get; set; } 

        [Required]
        public int EmpID { get; set; } 

        [Required]
        public int StockID { get; set; } 

        [Required]
        public int Payer { get; set; } 

        [Required]
        public DateTime CreateDate { get; set; }

        [Required]
        public DateTime InstockDate { get; set; } 

        [Required]
        public DateTime PayDate { get; set; } 

        [Required]
        public float Tax { get; set; } 

        [Required]
        public float Total { get; set; } 

        [Required]
        [MaxLength(10)]
        public string CurrencyUnit { get; set; } 

        [Required]
        public bool isPay { get; set; } 
    }
}
