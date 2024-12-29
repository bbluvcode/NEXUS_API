using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NEXUS_API.Models
{
    public class Discount
    {
        [Key]
        public string DiscountId { get; set; }
        public string DiscountName { get; set; } 
        [Column(TypeName = "decimal(10,2)")]
        public decimal DiscountPercentage { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; } 
        public string ApplyTo { get; set; }
        public ICollection<Equipment>? Equipments { get; set; }
    }
}
