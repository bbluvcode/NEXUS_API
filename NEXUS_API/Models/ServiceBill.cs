using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace NEXUS_API.Models
{
    public class ServiceBill
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BillId { get; set; }
        [Required]
        [StringLength(15)]
        public string Payer { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public DateTime? PayDate { get; set; }
        [Column(TypeName = "decimal(10,2)")]
        public decimal Tax { get; set; }
        [Column(TypeName = "decimal(10,2)")]
        public decimal Total { get; set; }
        public bool IsPay { get; set; }

        //Relationship
        [Required]
        public int ServiceOrderId { get; set; }
        public ServiceOrder? ServiceOrder { get; set; }
        public ICollection<ServiceBillDetail>? ServiceBillDetails { get; set; }
    }
}
