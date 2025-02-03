using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NEXUS_API.Models
{
    public class PlanFee
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PlanFeeId { get; set; }
        [Required]
        [StringLength(50)]
        public string PlanFeeName { get; set; }
        [Required]
        [StringLength(1000)]
        public string Description { get; set; }
        [Required]
        public bool IsUsing { get; set; }
        [Column(TypeName = "decimal(10,2)")]
        public decimal Rental { get; set; }
        [Column(TypeName = "decimal(10,2)")]
        public decimal CallCharge { get; set; }
        [Column(TypeName = "decimal(10,2)")]
        public decimal DTDCallCharge { get; set; }
        [Column(TypeName = "decimal(10,2)")]
        public decimal MessageMobileCharge { get; set; }
        [Column(TypeName = "decimal(10,2)")]
        public decimal LocalCallCharge { get; set; }

        //Relationship
        [Required]
        public int PlanId { get; set; }
        public Plan? Plan { get; set; }
    }
}
