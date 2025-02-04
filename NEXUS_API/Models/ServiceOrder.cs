using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NEXUS_API.Models
{
    public class ServiceOrder
    {
        [Key]
        [MaxLength(11)]
        public string OrderId { get; set; }
        public DateTime? DateCreate { get; set; }
        [Column(TypeName = ("decimal(10,2)"))]
        public decimal? Deposit { get; set; }
        public int? EmpIDCreater { get; set; }
        public Employee? EmployeeCreator { get; set; }
        public int? EmpIDSurveyor { get; set; }
        public Employee? EmployeeSurveyor { get; set; }
        public DateTime? SurveyDate { get; set; }
        public string? SurveyStatus { get; set; } //not yet, invalid, valid
        public string? SurveyDescribe { get; set; }

        //Relationship
        public string? AccountId { get; set; }
        public Account? Account { get; set; }
        public int? RequestId { get; set; }
        public CustomerRequest? CustomerRequest { get; set; }
        public int? PlanFeeId { get; set; }
        public PlanFee? PlanFee { get; set; }
        public ServiceBill? ServiceBill { get; set; }
        public ICollection<Connection>? Connections { get; set; }
        public InstallationOrder? InstallationOrder { get; set; }
    }
}
