using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace NEXUS_API.Models
{
    public class Vendor
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int VendorId { get; set; }
        [Required]
        [StringLength(15)]
        public string VendorName { get; set; }
        [Required]
        [StringLength(50)]
        public string Address { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [StringLength(10)]
        public string Phone { get; set; }
        [Required]
        [StringLength(15)]
        public string Fax { get; set; }
        [Required]
        [StringLength(1000)]
        public string Description { get; set; }

        //Relationship
        public ICollection<Equipment>? Equipments { get; set; }
    }
}
