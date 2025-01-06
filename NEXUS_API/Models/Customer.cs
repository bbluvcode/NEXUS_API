using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NEXUS_API.Models
{
    public class Customer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CustomerId { get; set; }

        [Required]
        [StringLength(50)]
        public string FullName { get; set; }

        [Required]
        [StringLength(7)]
        public string? Gender { get; set; }

        [Required]
        public DateTime? DateOfBirth { get; set; }

        [Required]
        [StringLength(200)]
        public string Address { get; set; }

        [StringLength(50)]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(20)]
        public string PhoneNumber { get; set; }

        [Required]
        [StringLength(20)]
        public string? IdentificationNo { get; set; }

        [StringLength(50)]
        public string? Image { get; set; }
        public string? Password { get; set; }

        //Relationship
        public ICollection<CustomerRequest>? CustomerRequests { get; set; }
        public ICollection<Account>? Accounts { get; set; }
        public ICollection<FeedBack>? FeedBacks { get; set; }
    }
}
