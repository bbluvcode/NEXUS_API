using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NEXUS_API.Models
{
    public class Plan
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PlanID { get; set; }

        [Required]
        [StringLength(50)]
        public string PlanName { get; set; }

        [Required]
        public decimal SecurityDeposit { get; set; } 

        [Required]
        [StringLength(1000)]
        public string Description { get; set; }

        [Required]
        public bool isUsing { get; set; }
    }
}
