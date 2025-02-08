namespace NEXUS_API.Models
{
    public class SupportRequestResponse
    {
        public int SupReqId { get; set; }
        public string ToEmail { get; set; }
        public string Subject { get; set; }
        public string ResponseContent { get; set; }
        public string CustomerName { get; set; }
    }
}
