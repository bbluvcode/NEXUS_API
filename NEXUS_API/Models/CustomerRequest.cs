using NEXUS_API.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NEXUS_API.Models
{
    public class CustomerRequest
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RequestId { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Request Title must not exceed 100 characters.")]
        public string RequestTitle { get; set; }

        [Required]
        [StringLength(1000, ErrorMessage = "Service Request must not exceed 1000 characters.")]
        public string ServiceRequest { get; set; } //plan fee

        public string? EquipmentRequest { get; set; } //equipment expect
        [Required]
        public DateTime? DateCreate { get; set; }
        public DateTime? DateResolve { get; set; }

        [Required]
        public bool IsResponse { get; set; } // Indicates if the request has been responded to
        [Column(TypeName = ("decimal(10,2)"))]
        public decimal Deposit { get; set; }
        public string? DepositStatus { get; set; } = "pending";
        public string? InstallationAddress { get; set; }
        //Relationship
        [Required]
        public int RegionId { get; set; }
        public Region? Region { get; set; }
        public int? CustomerId { get; set; }
        public Customer? Customer { get; set; }
    }
}