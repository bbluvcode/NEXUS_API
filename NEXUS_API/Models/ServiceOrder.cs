using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NEXUS_API.Models
{
    public class ServiceOrder
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderId { get; set; }
        public int CustomerId { get; set; }  //FK
        public int PlanFeeId { get; set; }
        public DateTime DateCreate { get; set; }
        [Column(TypeName =("decimal(10,2)"))]
        public decimal Deposit { get; set; }
        public int EmpIDSurveyor { get; set; }
        public DateTime SurveyDate { get; set; }
        public string SurveyStatus { get; set; } //not yet, invalid, valid
        public string SurveyDescribe { get; set; }
    }
}
