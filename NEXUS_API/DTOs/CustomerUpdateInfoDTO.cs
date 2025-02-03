namespace NEXUS_API.DTOs
{
    public class CustomerUpdateInfoDTO
    {
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string Gender { get; set; }
        public string Address { get; set; }
        public DateTime? DOB { get; set; }
    }
}