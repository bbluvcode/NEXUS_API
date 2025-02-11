using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace NEXUS_API.Models
{
    public class SupportRequest
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
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
        public bool IsResolved { get; set; } = false;
        public string? ResponseContent { get; set; }
        public string? CustomerName { get; set; }

        //Relationship
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public Customer? Customer { get; set; }
        public int? EmpIdResolver { get; set; }
        public Employee? Employee { get; set; }
    }
}