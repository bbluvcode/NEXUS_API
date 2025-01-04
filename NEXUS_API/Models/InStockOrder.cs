using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NEXUS_API.Models
{
    public class InStockOrder
    {
        [Key]
        public int InStockOrderId { get; set; } 

        [Required]
        public int InStockRequestId { get; set; } 

        [Required]
        public int VendorId { get; set; } 

        [Required]
        public int EmployeeId { get; set; } 

        [Required]
        public int StockId { get; set; } 

        [Required]
        public int Payer { get; set; } 

        [Required]
        public DateTime CreateDate { get; set; }

        [Required]
        public DateTime InstockDate { get; set; } 

        [Required]
        public DateTime PayDate { get; set; } 

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Tax { get; set; } 

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Total { get; set; } 

        [Required]
        [MaxLength(10)]
        public string CurrencyUnit { get; set; } 

        [Required]
        public bool isPay { get; set; }

        //Relationship
        public InStockRequest? InStockRequest { get; set; }
        public Vendor? Vendor { get; set; }
        public Employee? Employee { get; set; }
        public Stock? Stock { get; set; }
        public ICollection<InStockOrderDetail>? InStockOrderDetails { get; set; }
    }
}
