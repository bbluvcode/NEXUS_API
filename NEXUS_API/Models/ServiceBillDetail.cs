using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace NEXUS_API.Models
{
    public class ServiceBillDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BillDetailId { get; set; }
        public int BillId { get; set; }
        [Column(TypeName = "decimal(10,2)")]
        public decimal Deposit { get; set; }
        [Column(TypeName = "decimal(10,2)")]
        public decimal Discount { get; set; }
        [Column(TypeName = "decimal(10,2)")]
        public decimal Rental { get; set; }
        [Column(TypeName = "decimal(10,2)")]
        public decimal RentalDiscount { get; set; }
        [Column(TypeName = "decimal(10,2)")]
        public decimal CallCharge { get; set; }
        [Column(TypeName = "decimal(10,2)")]
        public decimal CallChargeTime { get; set; }
        [Column(TypeName = "decimal(10,2)")]
        public decimal LocalCallCharge { get; set; }
        [Column(TypeName = "decimal(10,2)")]
        public decimal LocalTime { get; set; }
        [Column(TypeName = "decimal(10,2)")]
        public decimal STDCallCharge { get; set; }
        [Column(TypeName = "decimal(10,2)")]
        public decimal STDTime { get; set; }
        [Column(TypeName = "decimal(10,2)")]
        public decimal MessageMobileCharge { get; set; }
        [Column(TypeName = "decimal(10,2)")]
        public decimal MessageMobileTime { get; set; }
        [Column(TypeName = "decimal(10,2)")]
        public decimal ServiceDiscount { get; set; }
    }
}
