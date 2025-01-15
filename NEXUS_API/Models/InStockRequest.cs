using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace NEXUS_API.Models
{
    public class InStockRequest
    {
        [Key]
        public int InStockRequestId { get; set; } 

        [Required]
        public int EmployeeId { get; set; } 

        [Required]
        public DateTime CreateDate { get; set; } 

        [Required]
        public int TotalNumber { get; set; }

        //Relationship
        public Employee? Employee { get; set; }
        public ICollection<InStockRequestDetail>? InStockRequestDetails { get; set; }
        public ICollection<InStockOrder>? InStockOrders { get; set; }
    }
}
