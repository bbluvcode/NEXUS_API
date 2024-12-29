using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace NEXUS_API.Models
{
    public class InStockRequest
    {
        [Key]
        public int ISRID { get; set; } 

        [Required]
        public int EmpID { get; set; } 

        [Required]
        public DateTime CreateDate { get; set; } 

        [Required]
        public int TotalNumber { get; set; }
    }
}
