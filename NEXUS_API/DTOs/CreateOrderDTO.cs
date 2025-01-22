namespace NEXUS_API.DTOs
{
    public class CreateOrderDTO
    {
        public int RequestId { get; set; }
        public decimal Deposit { get; set; }
        public int EmpIDCreater { get; set; }
        public int PlanFeeId { get; set; }
    }
}
