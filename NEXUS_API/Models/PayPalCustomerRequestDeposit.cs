using System.ComponentModel.DataAnnotations.Schema;

namespace NEXUS_API.Models
{
    public class PayPalCustomerRequestDeposit
    {
        public int Id { get; set; }
        public string CustomerRequestDepositId { get; set; }
        public string UserName { get; set; }
        [Column(TypeName = "decimal(10,2)")]
        public decimal Amount { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; }
    }
}
