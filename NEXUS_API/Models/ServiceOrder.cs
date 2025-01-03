using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NEXUS_API.Models
{
    public class ServiceOrder
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderId { get; set; }
        public DateTime DateCreate { get; set; }
        [Column(TypeName =("decimal(10,2)"))]
        public decimal Deposit { get; set; }
        public int EmpIDSurveyor { get; set; }
        public DateTime SurveyDate { get; set; }
        public string SurveyStatus { get; set; } //not yet, invalid, valid
        public string SurveyDescribe { get; set; }

        //Relationship
        [Required]
        public int CustomerId { get; set; }
        public Customer? Customer { get; set; }
        public ServiceBill? ServiceBill { get; set; }
        public ICollection<ServiceBillDetail>? ServiceBillDetails { get; set; }
        public ICollection<SupportRequest>? SupportRequests { get; set; }
    }
}
