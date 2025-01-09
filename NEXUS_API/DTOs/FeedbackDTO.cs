using NEXUS_API.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace NEXUS_API.DTOs
{
    public class FeedbackDTO
    {

        public int FeedBackId { get; set; }

        public string Title { get; set; }

        public string FeedBackContent { get; set; }

        public bool Status { get; set; }

        public int CustomerId { get; set; }
        public string FullName { get; set; }
        public string? Gender { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public string Address { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }
    }
}
