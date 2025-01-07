using System.ComponentModel.DataAnnotations;
namespace NEXUS_API.Models
{
    public class SupportRequest
    {
        [Key]
        public int SupportRequestId { get; set; }

        [Required(ErrorMessage = "Date of request is required")]
        public DateTime DateRequest { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [StringLength(200, ErrorMessage = "Title cannot be longer than 200 characters")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Detail content is required")]
        [StringLength(1000, ErrorMessage = "Content cannot be longer than 1000 characters")]
        public string DetailContent { get; set; }
        public DateTime? DateResolved { get; set; }
        public bool IsResolved { get; set; }

        //Relationship
        [Required]
        public int CustomerId { get; set; }
        public Customer? Customer { get; set; }
        public int? EmpIdResolver { get; set; }
        public Employee? Employee { get; set; }
    }
}