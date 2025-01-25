using System.ComponentModel.DataAnnotations;

namespace NEXUS_API.DTOs
{
    public class CustomerRequestDTO
    {
        public int RequestId { get; set; }
        public string RequestTitle { get; set; }
        public string ServiceRequest { get; set; }
        public string EquipmentRequest { get; set; }
        public DateTime? DateCreate { get; set; }
        public bool IsResponse { get; set; } // Indicates if the request has been responded to
        public int RegionId { get; set; }
        public string RegionCode { get; set; }
        public string RegionName { get; set; } // Name of region (varchar(20))

        public decimal? Deposit { get; set; }
        public string? DepositStatus { get; set; }
        public string? InstallationAddress { get; set; }

        public string FullName { get; set; }
        public string? Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }    
    }
}
