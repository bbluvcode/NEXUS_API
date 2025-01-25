namespace NEXUS_API.DTOs
{
    public class RequestShort
    {
        public int RequestId { get; set; }
        public string RequestTitle { get; set; }
        public string ServiceRequest { get; set; }
        public int RegionId { get; set; }
        public DateTime? DateCreate { get; set; }
        public bool IsResponse { get; set; }
    }
}
