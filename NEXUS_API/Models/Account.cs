using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace NEXUS_API.Models
{
    public class Account
    {
        [Key]
        public string AccountId { get; set; } 
        [Required]
        public int CustomerId { get; set; }
        public Customer? Customer { get; set; } 
        [Required]
        public string Type { get; set; }
        public DateTime? CreatedDate { get; set; }
        public ICollection<ServiceOrder>? ServiceOrders { get; set; }
    }
}
