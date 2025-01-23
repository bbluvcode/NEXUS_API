using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace NEXUS_API.DTOs
{
    public class ServiceBillDetailDTO
    {
        public decimal Deposit { get; set; }
        public decimal Discount { get; set; }
        public decimal Rental { get; set; }
        public decimal RentalDiscount { get; set; }
        public decimal CallCharge { get; set; }
        public decimal CallChargeTime { get; set; }
        public decimal LocalCallCharge { get; set; }
        public decimal LocalTime { get; set; }
        public decimal STDCallCharge { get; set; }
        public decimal STDTime { get; set; }
        public decimal MessageMobileCharge { get; set; }
        public decimal MessageMobileTime { get; set; }
        public decimal ServiceDiscount { get; set; }
    }
}
