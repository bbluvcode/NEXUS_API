namespace NEXUS_API.DTOs
{
    public class ServiceOrderDTO
    {
        public string AccountId { get; set; }
        public int EmpIDCreater { get; set; }
        public decimal Deposit { get; set; }
        public int EmpIDSurveyor { get; set; }
        public DateTime SurveyDate { get; set; }
    }
}
