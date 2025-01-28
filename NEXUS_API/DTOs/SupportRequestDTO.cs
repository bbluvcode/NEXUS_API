using System.ComponentModel.DataAnnotations;

namespace NEXUS_API.DTOs
{
    public class SupportRequestDTO
    {
        public int SupportRequestId { get; set; }

        public DateTime DateRequest { get; set; }
        public string Email { get; set; }


        public string Title { get; set; }

        public string DetailContent { get; set; }
        public DateTime? DateResolved { get; set; }
        public bool IsResolved { get; set; } = false;
        public int? CustomerId { get; set; }
        public string? FullName { get; set; }
        public string? Gender { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public string? Address { get; set; }


        public string? PhoneNumber { get; set; }
    }
}
