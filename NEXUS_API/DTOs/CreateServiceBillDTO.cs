using System.ComponentModel.DataAnnotations;

namespace NEXUS_API.DTOs
{
    public class CreateServiceBillDTO
    {
        [Required]
        public string Payer { get; set; } 
        [Required]
        public DateTime FromDate { get; set; } 
        [Required]
        public DateTime ToDate { get; set; } 
        [Required]
        public decimal TaxRate { get; set; }
        public List<ServiceBillDetailDTO>? Details { get; set; } 
    }
}
